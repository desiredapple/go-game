using GoGame.ViewModels;
using System.Collections.Generic;
using System;

namespace GoGame.Models;
public static class Engine
{
    //private bool WouldViolateKo(Board board, CellStatus status, int x, int y)
    //{
    //    var temp = board.CreateEmptyField(board.Size);
    //    board.CopyField(_field, temp);
    //    temp[x][y] = new Stone();
    //    return board.FieldsAreEqual(tempField, _prePreviousField);
    //}

    //�������� ������ ��� �������
    public static void RemoveCapturedStones(Board board,Stone currentStone)
    {
        CellStatus opponentStone = currentStone.Status == CellStatus.Black ? CellStatus.White : CellStatus.Black; //���������� ��������������� ���� �� �������
        var stonesToRemove = new List<Tuple<int, int>>();

        bool[,] visited = new bool[board.Size, board.Size]; //

        for (int x = 0; x < board.Size; ++x)
        {
            for (int y = 0; y < board.Size; ++y)
            {
                if (board[x, y].Status == opponentStone && !visited[x, y]) //���� ������ �� ����� ���� ����� � �� �������
                {
                    List<Tuple<int, int>> group = new List<Tuple<int, int>>();
                    if (!HasLiberties(board, x, y, opponentStone, visited, group)) //������� ���� � ����������� ������ ������� �� �������
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

    private static bool HasLiberties(Board board, int x, int y, CellStatus status, bool[,] visited, List<Tuple<int, int>> group)
    {
        int size = 19;
        if (x < 0 || x >= size || y < 0 || y >= size || visited[x, y]) //visted ok, �� ��� �� ������ �� �������?
            return false;

        if (board[x, y] == null)
            return true;

        if (board[x, y].Status != status)
            return false;

        visited[x, y] = true;
        group.Add(new Tuple<int, int>(x, y));

        bool hasLiberty = false;
        //���� ���� ���� �� ���� ������� � �������� ������
        hasLiberty |= HasLiberties(board, x + 1, y, status, visited, group);
        hasLiberty |= HasLiberties(board, x - 1, y, status, visited, group);
        hasLiberty |= HasLiberties(board, x, y + 1, status, visited, group);
        hasLiberty |= HasLiberties(board, x, y - 1, status, visited, group);

        return hasLiberty;
    }
}
