// количество клеток на доске, используется для инициализации класса Board
public enum BoardSize : byte 
{
    Ultrasmall,     // 5x5
    Small,          // 7x7
    Medium,         // 15x15
    Default         // 19x19
}


namespace Board
{
    public class Board
    {
        public Board(BoardSize size)
        {
            InitializeBoard(size);
        }
        private Stone[,] field;

        void InitializeBoard(Boardsize boardsize)
        {


            int size;

            switch (boardsize)
            {
                case BoardSize.Ultrasmall:
                    size = 5;
                    break;
                case BoardSize.Small:
                    size = 7;
                    break;
                case BoardSize.Medium:
                    size = 15;
                    break;
                case BoardSize.Default:
                    size = 19;
                    break;  
                default:
                    return;


            }

            field = new Stone[size,size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    field[x,y].X = x;
                    field[x,y].Y = y;
                    field[x,y].IsDead = true; //изначально камушек мертвый(
                }
            }
        }
    }
}
