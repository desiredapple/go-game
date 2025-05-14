using GoGame.ViewModels;

public enum CellStatus : byte
{
    Black,
    White,
    Empty
}

namespace GoGame.Models
{
    public class Stone
    {
        private readonly CellStatus _status;
        private int _x;
        private int _y;

        public CellStatus Status { get { return _status; } }

        public Stone()
        {
            _status = (Board._globalMoveCounter % 2 == 0)
                    ? CellStatus.Black
                    : CellStatus.White;
        }

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
