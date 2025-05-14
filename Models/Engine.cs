using GoGame.ViewModels;
using System.Collections.Generic;
using System;

namespace GoGame.Models;
public static class Engine
{
    public static void RemoveCapturedStones(Board board, Stone currentStone)
    {
        StoneColor opponentStone = currentStone.Status == StoneColor.Black ? StoneColor.White : StoneColor.Black; //записываем противоположным цвет от данного
        var stonesToRemove = new List<Tuple<int, int>>();

        bool[,] visited = new bool[board.Size, board.Size];

        for (int x = 0; x < board.Size; ++x)
        {
            for (int y = 0; y < board.Size; ++y)
            {
                if (board[x, y] != null && board[x, y].Status == opponentStone && !visited[x, y]) //если камень на доске прот цвета и не посещен
                {
                    List<Tuple<int, int>> group = [];
                    if (!HasLiberties(board, x, y, opponentStone, visited, group)) //смотрим если у конкретного камней ДЫХАНИЕ см правила
                    {
                        stonesToRemove.AddRange(group);
                    }
                }
            }
        }

        foreach (var stone in stonesToRemove)
        {
            board[stone.Item1, stone.Item2] = null;        
        }
    }

    public static bool HasLiberties(Board board, int x, int y, StoneColor status, bool[,] visited, List<Tuple<int, int>> group)
    {
        int size = 19;
        if (x < 0 || x >= size || y < 0 || y >= size || visited[x, y])
            return false;

        if (board[x, y] == null)
            return true;

        if (board[x, y].Status != status)
            return false;

        visited[x, y] = true;
        group.Add(new Tuple<int, int>(x, y));

        bool hasLiberty = false;
        //если есть хотя бы одно дыхание в соседнях клетка
        hasLiberty |= HasLiberties(board, x + 1, y, status, visited, group);
        hasLiberty |= HasLiberties(board, x - 1, y, status, visited, group);
        hasLiberty |= HasLiberties(board, x, y + 1, status, visited, group);
        hasLiberty |= HasLiberties(board, x, y - 1, status, visited, group);

        return hasLiberty;
    }
}
