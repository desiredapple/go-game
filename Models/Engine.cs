using System;
using GoGame.ViewModels;

namespace GoGame.Models;


public class Engine
{
    public void PlaceStone(Board board, Stone stone, int x, int y)
    {
        //if (WouldViolateKo(board, stone.Status, x, y))
            //throw new InvalidOperationException("Нарушение правила Ко");
        board.SaveHistory();
        //_field[x][y] = new Stone();
        stone.SetPosition(x, y);
    }

    //private bool WouldViolateKo(Board board, CellStatus status, int x, int y)
    //{
        //var temp = board.CreateEmptyField(board.Size);
        //board.CopyField(_field, temp);
        //temp[x][y] = new Stone();
        //return board.FieldsAreEqual(tempField, _prePreviousField);
    //}
}
