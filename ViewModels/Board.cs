using GoGame.Models;
using System;


namespace GoGame.ViewModels;

public class Board
{
    private Stone[][] _field;
    private Stone[][] _previousField;
    private Stone[][] _prePreviousField;
    private int _size;
    public static int _globalMoveCounter;

    public int Size {  get { return _size; } }

    public Board()
    {
        _field = CreateEmptyField(_size);
        _previousField = CreateEmptyField(_size);
        _prePreviousField = CreateEmptyField(_size);
    }

    public void CopyField(Stone[][] source, Stone[][] clone)
    {
        for (int i = 0; i < _size; ++i)
        {
            Array.Copy(source[i], clone[i], _size);
        }
    }


    public Stone[][] CreateEmptyField(int size)
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
}
