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
            switch (size)
            {
                case BoardSize.Ultrasmall:
                    field = new int[5][5];
                    break;
                case BoardSize.Small:
                    field = new int[7][7];
                    break;
                case BoardSize.Medium:
                    field = new int[15][15];
                    break;
                case BoardSize.Default:
                    field = new int[19][19];
                    break;
            }
        }
        private int field[][];
    }
}
