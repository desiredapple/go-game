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
}
