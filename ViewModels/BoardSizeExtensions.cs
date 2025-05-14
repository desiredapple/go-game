using Board
namespace BoardSizeExtensions
{
    public static class BoardSizeExtensions
    {
        public static int GetSize(BoardSize size)
        {
            return size switch
            {
                BoardSize.Ultrasmall => 5,
                BoardSize.Small => 7,
                BoardSize.Medium => 15,
                BoardSize.Default => 19,
                => throw new ArgumentOutOfRangeException(nameof(size), $"Unknown board size: {size}")
            };
        }
    }
}
