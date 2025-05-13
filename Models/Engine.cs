using GoGame.ViewModels;

namespace GoGame.Models

{
    public class Engine
    {
        public void PlaceStone(Stone stone, int x, int y)
        {
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                throw new ArgumentOutOfRangeException(nameof(x), "Координаты вне доски");
            if (_field[x][y] != 0)
                throw new InvalidOperationException("Клетка уже занята");
            if (WouldViolateKo(x, y, stone.Color))
                throw new InvalidOperationException("Нарушение правила Ко");
            SaveHistory();
            _field[x][y] = stone.Status == CellStatus.Black ? 1 : 2;
            stone.SetPosition(x, y);
        }

        private bool WouldViolateKo(int x, int y, CellStatus status)
        {
            var temp = CreateEmptyField(_size);
            CopyField(_field, temp);
            temp[x][y] = status == CellStatus.Black ? 1 : 2;
            return FieldsEqual(tempField, _prePreviousField);
        }
    }
}
