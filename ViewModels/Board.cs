using GoGame.Models;
using System;
using System.ComponentModel;

namespace GoGame.ViewModels;

public class Board : INotifyPropertyChanged
{
    private int _size;
    private Stone[][] __field;
    private Stone[][] _previousField;
    private Stone[][] _prePreviousField;
    public int _moveCounter = 0;
    public event PropertyChangedEventHandler PropertyChanged;
    public int Size { get { return _size; } }

    private static const float Komi = 7.5f;  //число доп очков, которое белые получают за то что черные ходят первые

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
        _previousField = CreateEmptyField(size);
        _prePreviousField = CreateEmptyField(size);
    }

    public void CopyField(Stone[][] source, Stone[][] clone)
    {
        for (int i = 0; i < _size; ++i)
        {
            Array.Copy(source[i], clone[i], _size);
        }
    }

    public static Stone[][] CreateEmptyField(int size)
    {
        var f = new Stone[size][];
        for (int i = 0; i < size; ++i)
            f[i] = new Stone[size];
        return f;
    }

    public bool FieldsAreEqual(Stone[][] a, Stone[][] b)
    {
        for (int x = 0; x < _size; ++x)
            for (int y = 0; y < _size; ++y)
                if (a[x][y] != b[x][y])
                    return false;
        return true;
    }

    public void SaveHistory()
    {
        CopyField(_previousField, _prePreviousField);
        CopyField(_field, _previousField);
    }



    public (blackScore, whiteScore) ScoringPoints()
    {
        
        int blackStones = 0;
        int whiteStones = 0;
        int blackTerritory = 0;
        int whiteTerritory = 0;

        var visited = new bool[_size, _size];

        // Подсчет камней на доске
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; x < _size; y++)
            {
                if (_field[x, y].Status == CellStatus.Black) blackStones++;
                else if (_field[x, y].Status == CellStatus.White) whiteStones++;
            }
        }


        
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                if (_field[x, y].Status != CellStatus.Empty || visited[x, y]) continue;
                //если мы были уже в клетке ИЛИ она не пустая

                var (territoryColor, sizeofTerr) = AnalyzeTerritory(x, y, visited); //приватный метод вычисляющий цвет захваченной территории и ее размер
                if (territoryColor == CellStatus.Black) blackTerritory += sizeofTerr;
                else if (territoryColor == CellStatus.White) whiteTerritory += sizeofTerr;


            }
        }

        float blackScore = blackTerritory;
        float whiteScore = whiteTerritory + Komi;

        return (blackScore, whiteScore);

    }
    private (CellStatus territoryColor, int sizeofTerr) AnalyzeTerritory(int x, int y, bool[][] visited) 
    {
        var queue = new Queue<(int, int)>();
        queue = queue.Enqueue((x, y));
        visited[x, y] = true;

        CellStatus borderColor = CellStatus.Empty;
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

                if (_field[nx, ny] != CellStatus.Empty)
                {
                    if (borderColor == CellStatus.Empty)
                    {
                        borderColor = _field[nx, ny].Status; //меняем на первый встреченный цвет
                    }
                    else if (borderColor != _field[nx, ny].Status)
                    {
                        return (CellStatus.Empty, 0);
                    }
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
