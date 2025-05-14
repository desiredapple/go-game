namespace GoGame.Models;

public class Stone
{
    private readonly CellStatus _status;

    public CellStatus Status { get { return _status; } }

    public Stone(CellStatus status)
    {
        _status = status;
    }
}
