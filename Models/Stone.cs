namespace GoGame.Models;

public class Stone
{
    private readonly StoneColor _status;

    public StoneColor Status { get { return _status; } }
    public Stone(StoneColor status)
    {
        _status = status;
    }
}
