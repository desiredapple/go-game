public enum StoneColor : byte
{
    White,
    Black
}

namespace Stone
{
    public class Stone
    {
        public static int _globalMoveCounter = 0;

        // конструктор нового камня, без параметров, координаты выставляются в PlaceStone
        public Stone()
        {
            _color = (_globalMoveCounter % 2 == 0)
                    ? StoneColor.Black
                    : StoneColor.White;

            // пока координаты фиктивные, сначала нужно проверить легальность хода
            _x = -1;
            _y = -1;
        }
        public StoneColor Color
        {
            get => _color;
        }
        public int X
        {
            get => _x;
        }
        public int Y
        {
            get => _x;
        }
        // функция для выставления координатов камня после проверки легальности хода
        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
            _globalMoveCounter++;
        }

        private readonly StoneColor _color;
        private int _x;
        private int _y;
    }
}
