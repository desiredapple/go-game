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

        private void InitializeBoard(Boardsize boardsize)
        {


            int size = boardsize.GetSize();

            

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

        public Pair<float,float> ScoringPoints(Boardsize boardsize)
        {
            int size = boardsize.GetSize();
            int blackStones = 0;
            int whiteStones = 0;
            int blackTerritory = 0;
            int whiteTerritory = 0;

            var visited = new bool[BoardSize, BoardSize];

            // Подсчет камней на доске
            for (int x = 0; x < size; y++)
            {
                for (int y = 0; x < size; x++)
                {
                    if (field[x, y].Color == StoneColor.Black) blackStones++;
                    else if (field[x, y].Color == StoneColor.White) whiteStones++;
                }
            }
        }
    }
}
