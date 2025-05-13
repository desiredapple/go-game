
namespace BoardSizeExtensions
{
    public static class BoardSizeExtensions
    {
        public static int GetSize(this BoardSize size)
        {
            return size switch
            {
                BoardSize.Ultrasmall => 5,
                BoardSize.Small => 7,
                BoardSize.Medium => 15,
                BoardSize.Default => 19,
                _ => throw new ArgumentOutOfRangeException(nameof(size), $"Unknown board size: {size}")
            };
        }
    }
}
