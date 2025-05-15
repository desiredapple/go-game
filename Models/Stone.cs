namespace GoGame.Models;

public class Stone
{
    private readonly StoneColor _color;

    public StoneColor Color { get { return _color; } }
    public Stone(StoneColor status)
    {
        _color = status;
    }
}
