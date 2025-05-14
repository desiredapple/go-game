namespace GoGame.Models;

public class Stone
{
    private readonly CellStatus _status;
    private readonly int _x;
    private readonly int _y;

    public CellStatus Status { get { return _status; } }

    public Stone(CellStatus status, int x, int y)
    {
        _x = x;
        _y = y;
        _status = status;

            }
            public int Y
            {
                get => _x;
            }
            
      

            private readonly StoneColor _color;
            private int _x;
            private int _y;
}
