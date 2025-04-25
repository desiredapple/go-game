public enum StoneColor : byte
{
    White,
    Black
}

namespace Stone
{
    public class Stone
    {
        public Stone()
        {
            //Некоторая переменная i, отвечающая за нумерацию ходов. Т. к. черные начинают первые, то
            //четные ходы делают черные, нечетные — белые
            //color = i % 2 ? StoneColor.White : StoneColor.Black;
        }
        public StoneColor Color
        {
            get => _color;
        }
        private readonly StoneColor _color;
        private int _x;
        private int _y;
    }
}
