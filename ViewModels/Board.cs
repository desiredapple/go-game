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
        public Board(BoardSize _size)
        {
            InitializeBoard(_size);
        }
        private Stone[,] field;
        private int _size;

        private void InitializeBoard(Boardsize boardsize)
        {


            

            

            field = new Stone[_size,_size];

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    field[x,y].X = x;
                    field[x,y].Y = y;
                     //изначально пересечение пустое
                }
            }
        }

        //скорее всего позже уйдет под другой класс
        public (float blackScore,float whiteScore) ScoringPoints(Boardsize boardsize)
        {
            
            int blackStones = 0;
            int whiteStones = 0;
            int blackTerritory = 0;
            int whiteTerritory = 0;

            var visited = new bool[BoardSize, BoardSize];

            // Подсчет камней на доске каждого цвета
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; x < _size; y++)
                {
                    if (field[x, y].Color == StoneColor.Black) blackStones++;
                    else if (field[x, y].Color == StoneColor.White) whiteStones++;
                }
            }


            //нужна будет допфункция анализа территории BFS промт инженеринг подсказал
            for (int x = 0;x < _size; x++)
            {
                for (int y = 0;y < _size; y++)
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


        private (StoneColor territoryColor,int size) AnalyzeTerritory(int x, int y, bool[][] visited) //теку
        {
            var queue = new Queue<(int , int y)>();
            queue = queue.Enqueue((x, y));
            visited[x, y] = true;

            StoneColor borderColor = StoneColor.None;
            int territorySize = 0;
            
            //обход территории, BFS - поиск в ширину
            while (queue.Count > 0)
            {
                var (currentX, currentY) = queue.Dequeue();
                territorySize++;

                //проверка все соседних точек   
                foreach (var (dx, dy) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                {
                    int nx = currentX + dx;
                    int ny = currentY + dy;

                    //проверка выхода за границу доски
                    if (nx < 0 || nx >= _size || ny<0 || ny >= _size) continue;

                    if (field[nx, ny] != StoneColor.None)
                    {
                        if (borderColor == StoneColor.None)
                        {
                            borderColor = field[nx, ny].Color; //меняем на первый встреченный цвет
                        }
                        else if (borderColor != _stones[nx, ny].Color)
                        {
                            return (StoneColor.None);
                        }
                    }

                    //если сосед - None color и не посещен
                    else if (!visited[nx, ny])
                    {
                        visited[nx, ny] = true;
                        queue.Enqueue((nx, ny));
                    }
                }
            }


            return (borderColor, territorySize);
        }
    }
}
