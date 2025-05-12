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

        // копирование доски
        private static void CopyField(int[][] source, int[][] clone)
        {
            for (int i = 0; i < source.Length; i++)
                Array.Copy(source[i], clone[i], source[i].Length);
        }

        // создать пустое поле 
        private static int[][] CreateEmptyField(int size)
        {
            var f = new int[size][];
            for (int i = 0; i < size; i++)
                f[i] = new int[size];
            return f;
        }

        // сравнение двух полей поэлементно
        private static bool FieldsEqual(int[][] a, int[][] b)
        {
            for (int x = 0; x < a.Length; x++)
                for (int y = 0; y < a.Length; y++)
                    if (a[x][y] != b[x][y])
                        return false;
            return true;
        }
    }
}
