using GoGame.Models;

namespace GoGame.ViewModels;

public class Board
{
    private CellStatus[,] _field;
    private CellStatus[,] _previousField;
    private CellStatus[,] _prePreviousField;
    private int _size;

    public Board(int size)
    {
        _size = size;
        _field = new CellStatus[size, size];
    }

    private void CopyField(CellStatus[,] source, CellStatus[,] clone)
    {
        for (int i = 0; i < _size; ++i)
            Array.Copy(source[i], clone[i], _size);
    }

    private CellStatus[,] CreateEmptyField(int size)
    {
        var f = new int[size][];
        for (int i = 0; i < size; ++i)
            f[i] = new int[size];
        return f;
    }

    private bool FieldsEqual(CellStatus[,] a, CellStatus[,] b)
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
