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
        public Stone()
        {
            //Некоторая переменная i, отвечающая за нумерацию ходов. Т. к. черные начинают первые, то
            //четные ходы делают черные, нечетные — белые
            // _color = i % 2 ? StoneColor.White : StoneColor.Black;
        }
        private readonly CellStatus _status;
        private int _x;
        private int _y;
    }
}
