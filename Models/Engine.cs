using GoGame.ViewModels;

namespace GoGame.Models;
public class Engine
{
    //public void PlaceStone(Board board, Stone stone, int x, int y)
    //{
    //    //if (WouldViolateKo(board, stone.Status, x, y))
    //    //throw new InvalidOperationException("Íàðóøåíèå ïðàâèëà Êî");
    //    board.SaveHistory();
    //    board[x, y] = new Stone(board.MoveCounter % 2 == 0 ? CellStatus.Black : CellStatus.White, x, y);
    //}

    //private bool WouldViolateKo(Board board, CellStatus status, int x, int y)
    //{
    //var temp = board.CreateEmptyField(board.Size);
    //board.CopyField(_field, temp);
    //temp[x][y] = new Stone();
    //return board.FieldsAreEqual(tempField, _prePreviousField);
    //}



    //удаление камней БЕЗ ДЫХАНИЯ
    public void RemoveCapturedStones(Stone currentStone)
    {
        int size = 19; //поставить размер поля

        
        CellStatus opponentStone = currentStone.Status == CellStatus.Black ? CellStatus.White : CellStatus.Black; //записываем противоположным цвет от данного
        List<Tuple<int, int>> stonesToRemove = new List<Tuple<int, int>>();

        bool[,] visited = new bool[size, size]; //

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                
                if (board[i, j].Status == opponentStone && !visited[i, j]) //если камень на доске прот цвета и не посещен
                {
                    List<Tuple<int, int>> group = new List<Tuple<int, int>>(); 
                    if (!HasLiberties(i, j, opponentStone, visited, group)) //смотрим если у конкретного камней ДЫХАНИЕ см правила
                    {
                        stonesToRemove.AddRange(group);
                    }
                }
            }
        }

        foreach (var stone in stonesToRemove)
        {
            board[stone.Item1, stone.Item2] = null; //here remove
        }
    }

    private bool HasLiberties(int x, int y, CellStatus status, bool[,] visited, List<Tuple<int, int>> group)
    {
        int size = 19;
        if (x < 0 || x >= size || y < 0 || y >= size || visited[x, y]) //visted ok, но как мы выйдем за пределы?
            return false;

        if (board[x, y].Status == CellStatus.Empty)
            return true;

        if (board[x, y].Status != status)
            return false;

        visited[x, y] = true;
        group.Add(new Tuple<int, int>(x, y));

        bool hasLiberty = false;
        //если есть хотя бы одно дыхание в соседнях клетка
        hasLiberty |= HasLiberties(x + 1, y, status, visited, group);
        hasLiberty |= HasLiberties(x - 1, y, status, visited, group);
        hasLiberty |= HasLiberties(x, y + 1, status, visited, group);
        hasLiberty |= HasLiberties(x, y - 1, status, visited, group);

        return hasLiberty;
    }

}
