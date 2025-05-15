using GoGame.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GoGame.ViewModels;

public class Board : INotifyPropertyChanged
{
    private readonly int _size;
    private readonly Stone[][] _field;
    private static readonly double Komi = 7.5;
    public int _moveCounter = 0;
    public event PropertyChangedEventHandler PropertyChanged;
    public int Size { get { return _size; } }

    public Stone this[int x, int y]
    {
        get => _field[x][y];
        set => _field[x][y] = value;
    }

    public int MoveCounter
    {
        get => _moveCounter;
        set
        {
            _moveCounter = value;
            OnPropertyChanged(nameof(MoveCounter));
        }
    }

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public Board(int size)
    {
        _size = size;
        _field = CreateEmptyField(size);
    }
    public static Stone[][] CreateEmptyField(int size)
    {
        var f = new Stone[size][];
        for (int i = 0; i < size; ++i)
            f[i] = new Stone[size];
        return f;
    }

    public (double blackScore, double whiteScore) ScoringPoints()
    {
        int blackStones = 0;
        int whiteStones = 0;
        int blackTerritory = 0;
        int whiteTerritory = 0;

        var visited = new bool[_size, _size];

        // 1. Подсчет камней на доске
        for (int x = 0; x < _size; ++x)
        {
            for (int y = 0; y < _size; ++y)
            {
                if (_field[x][y] != null)
                {
                    if (_field[x][y].Color == StoneColor.Black)
                        blackStones++;
                    else if (_field[x][y].Color == StoneColor.White)
                        whiteStones++;
                }
            }
        }

        // 2. Подсчет территорий для пустых клеток
        for (int x = 0; x < _size; ++x)
        {
            for (int y = 0; y < _size; ++y)
            {
                if (_field[x][y] == null && !visited[x, y]) // Пустая клетка, еще не посещенная
                {
                    // Определяем владельца территории для текущей пустой клетки
                    var territory = CheckTerritoryOwner(x, y, visited);

                    // Если территория принадлежит одному игроку, увеличиваем счет этого игрока
                    if (territory.Color == StoneColor.Black)
                        blackTerritory += territory.Item2;
                    else if (territory.Color == StoneColor.White)
                        whiteTerritory += territory.Item2;
                }
            }
        }

        // Подсчитываем общий счет
        double blackScore =   blackTerritory;
        double whiteScore = whiteTerritory + Komi; // Коми добавляем для белых

        return (blackScore, whiteScore);
    }

    // Метод для проверки, какая сторона окружила пустую клетку (черные или белые)
    // Возвращает пару: (StoneColor, кол-во клеток территории)
    private (StoneColor? Color, int) CheckTerritoryOwner(int x, int y, bool[,] visited)
    {
        var queue = new Queue<Tuple<int, int>>();
        queue.Enqueue(Tuple.Create(x, y));
        visited[x, y] = true;

        HashSet<StoneColor> surroundingColors = new HashSet<StoneColor>();
        List<Tuple<int, int>> territoryCells = new List<Tuple<int, int>>();

        // Направления для смежных клеток (вверх, вниз, влево, вправо)
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int cx = current.Item1;
            int cy = current.Item2;
            territoryCells.Add(current); // Добавляем эту клетку в территорию

            // Проверяем соседей текущей клетки
            for (int i = 0; i < 4; ++i)
            {
                int nx = cx + dx[i];
                int ny = cy + dy[i];

                // Если сосед выходит за пределы доски, пропускаем его
                if (nx < 0 || nx >= _size || ny < 0 || ny >= _size)
                    continue;

                // Если сосед уже посещен, пропускаем его
                if (visited[nx, ny])
                    continue;

                // Если сосед — это камень, добавляем его цвет в множество  
                if (_field[nx][ny] != null)
                {
                    surroundingColors.Add(_field[nx][ny].Color);
                }
                // Если сосед — пустая клетка, добавляем ее в очередь для дальнейшей проверки
                else
                {
                    visited[nx, ny] = true;
                    queue.Enqueue(Tuple.Create(nx, ny));
                }
            }
        }

        // Если территория окружена камнями одного цвета, возвращаем этот цвет и количество клеток в территории
        if (surroundingColors.Count == 1)
        {
            return (surroundingColors.First(), territoryCells.Count);
        }

        // Если территория окружена камнями разных цветов или не окружена камнями, то не является территорией
        return (null, 0);
    }

}
