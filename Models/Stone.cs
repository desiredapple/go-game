    public enum StoneColor : byte
    {
        White,
        Black,
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
                IsDead = false;
                
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
            
            public bool IsDead { get; set; } //проверка на поле ли камень/жив ли он

            private readonly StoneColor _color;
            private int _x;
            private int _y;
    }
