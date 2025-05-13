using System;
using GoGame.Models;

// количество клеток на доске, используется для инициализации класса Board
public enum BoardSize : byte
{
    Ultrasmall,     // 5x5
    Small,          // 7x7
    Medium,         // 15x15
    Default         // 19x19
}

namespace GoGame.ViewModels;

{
    private CellStatus[][] _field;
    private CellStatus[][] _previousField;
    private CellStatus[][] _prePreviousField;
    private int _size;

    public Board()
    {
        _field = CreateEmptyField(_size);
        _previousField = CreateEmptyField(_size);
        _prePreviousField = CreateEmptyField(_size);
    }

    private void CopyField(CellStatus[][] source, CellStatus[][] clone)
    {
        for (int i = 0; i < _size; ++i)
        {
            Array.Copy(source[i], clone[i], _size);
        }
    }


    private CellStatus[][] CreateEmptyField(int size)
    {
        var f = new CellStatus[size][];
        for (int i = 0; i < size; i++)
            f[i] = new CellStatus[size];
        return f;
    }

    private bool FieldsEqual(CellStatus[][] a, CellStatus[][] b)
    {
        for (int x = 0; x < _size; ++x)
            for (int y = 0; y < _size; ++y)
                if (a[x][y] != b[x][y])
                    return false;
        return true;
    }

    private void SaveHistory()
    {
        CopyField(_previousField, _prePreviousField);
        CopyField(_field, _previousField);
    }
}
