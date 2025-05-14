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
                    field[x,y].IsDead = true; //изначально пересечение пустое
                }
            }
        }

        //скорее всего позже уйдет под другой класс
        public (blackScore,whiteScore) ScoringPoints(Boardsize boardsize)
        {
            int size = boardsize.GetSize();
            int blackStones = 0;
            int whiteStones = 0;
            int blackTerritory = 0;
            int whiteTerritory = 0;

            var visited = new bool[BoardSize, BoardSize];

            // Подсчет камней на доске
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; x < size; y++)
                {
                    if (field[x, y].Color == StoneColor.Black) blackStones++;
                    else if (field[x, y].Color == StoneColor.White) whiteStones++;
                }
            }


            //нужна будет допфункция анализа территории BFS промт инженеринг подсказал
            for (int x = 0;x < size; x++)
            {
                for (int y = 0;y < size; y++)
                {
                    if (!_field[x, y].IsDead || visited[x, y]) continue;
                    //если мы были уже в клетке ИЛИ она умерла

                    var (territoryColor, size) = AnalyzeTerritory(x, y, visited); //приватный метод вычисляющий цвет захваченной территории и ее размер
                    if (territoryColor == StoneColor.Black) blackTerritory += size;
                    else if (territoryColor == StoneColor.White) whiteTerritory += size;


                }
            }

            float blackScore = blackTerritory + blackStones + _whitePrisoners;
            float whiteScore = whiteTerritory + whiteStones + _blackPrisoners + Komi;

            return (blackScore, whiteScore);

        }
    }
}
