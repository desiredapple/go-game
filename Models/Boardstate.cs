using Stone;
using Board;
using System;

namespace boardstate
{
    // класс для быстрой проверки состояния игроковой доски (0 = Empty, 1 = Black, 2 = White)
    public partial class BoardState
    {
        private readonly int _size;
        private int[][] _field;

        // храним две последние позиции доски для правила КО
        private int[][] _previousField;
        private int[][] _prePreviousField;
    }
}
