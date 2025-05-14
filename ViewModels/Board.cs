using GoGame.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        // Подсчет камней на доске
        for (int x = 0; x < _size; ++x)
        {
            for (int y = 0; y < _size; ++y)
            {
                if (_field[x][y] != null)
                {
                    if (_field[x][y].Status == StoneColor.Black) blackStones++;
                    else if (_field[x][y].Status == StoneColor.White) whiteStones++;
                }
            }
        }

        for (int x = 0; x < _size; ++x)
        {
            for (int y = 0; y < _size; ++y)
            {
                if (_field[x][y] != null || visited[x, y]) continue;

                var (territoryColor, sizeofTerr) = AnalyzeTerritory(x, y, visited); //приватный метод вычисляющий цвет захваченной территории и ее размер
                if (territoryColor == StoneColor.Black) blackTerritory += sizeofTerr;
                else if (territoryColor == StoneColor.White) whiteTerritory += sizeofTerr;
            }
        }

        double blackScore = 361 - blackTerritory;
        double whiteScore = whiteTerritory + Komi;

        return (blackScore, whiteScore);

    }
    private (StoneColor territoryColor, int sizeofTerr) AnalyzeTerritory(int x, int y, bool[,] visited)
    {
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));
        visited[x, y] = true;

        StoneColor borderColor = StoneColor.Black;
        int territorySize = 0;

        //обход территории, BFS - поиск в ширину
        while (queue.Count > 0)
        {
            var (currentX, currentY) = queue.Dequeue();
            territorySize++;

            //проверка все соседних точек   
            foreach (var (dx, dy) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                int nx = currentX + dx;
                int ny = currentY + dy;

                //проверка выхода за границу доски
                if (nx < 0 || nx >= _size || ny < 0 || ny >= _size) continue;

                if (_field[nx][ny] != null)
                {
                        borderColor = _field[nx][ny].Status; //меняем на первый встреченный цвет
                }

                //если сосед has Empty SellStatus и не посещен
                else if (!visited[nx, ny])
                {
                    visited[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }
        return (borderColor, territorySize);
    }
}
