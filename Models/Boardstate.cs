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

        // нужен size из класса Board.cs
        public BoardState(BoardSize size)
        {
            _size = size switch
            {
                BoardSize.Ultrasmall => 5,
                BoardSize.Small => 7,
                BoardSize.Medium => 15,
                _ => 19,
            };
            _field = CreateEmptyField(_size);
            _previousField = CreateEmptyField(_size);
            _prePreviousField = CreateEmptyField(_size);
        }

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

        // вызывается перед каждым новым реальным ходом, сохраняем историю
        private void SaveHistory()
        {
            CopyField(_previousField, _prePreviousField);
            CopyField(_field, _previousField);
        }

        // при нажатии на пункт, вызывается конструктор камня, а потом PlaceStone с этим камнем и координатами пункта
        public void PlaceStone(Stone stone, int x, int y)
        {
            // проверка чтобы пункт был в границах доски
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                throw new ArgumentOutOfRangeException(nameof(x), "Координаты вне доски");

            // проверка чтобы пункт был пустой
            if (_field[x][y] != 0)
                throw new InvalidOperationException("Клетка уже занята");

            // проверка правила Ко: симулируем ход и сравниваем с позапрошлой позицией
            if (WouldViolateKo(x, y, stone.Color))
                throw new InvalidOperationException("Нарушение правила Ко");


            // тут нужна проверка правила суицидального камня

            SaveHistory();
            
            // обновляем цвет пункта на доске
            _field[x][y] = stone.Color == StoneColor.Black ? 1 : 2;

            // меняем координаты на настоящие у камня 
            stone.SetPosition(x, y);

            // тут нужна функция с проверкой/реализацией сруба
            // тут нужно обновить нынешнее поле, если сруб проходит, убрать срубленные камни

        }
    }
}
