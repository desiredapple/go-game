using System;
using System.ComponentModel;
using System.Windows.Media;
using GoGame.Models;

namespace GoGame.ViewModels;

public class Board : INotifyPropertyChanged
{
    private int _size;
    private Stone[][] _field;
    private Stone[][] _previousField;
    private Stone[][] _prePreviousField;
    public int _moveCounter = 0;

    public event PropertyChangedEventHandler PropertyChanged;
    public int MoveCounter
    {
        get => _moveCounter;
        set
        {
            if (_moveCounter != value)
            {
                _moveCounter = value;
                OnPropertyChanged(nameof(MoveCounter));
            }
        }
    }

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public Stone this[int x, int y]
    {
        get => _field[x][y];
        set => _field[x][y] = value;
    }

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
}
