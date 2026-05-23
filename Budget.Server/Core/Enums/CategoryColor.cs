namespace Budget.Server.Core.Enums
{
    public enum CategoryColor
    {
        None = 0,
        Blue = 1,
        Green = 2,
        Yellow = 3,
        Orange = 4,
        Red = 5,
    }

    public static class CategoryColorExtension
    {
        public static string ToHex(this CategoryColor color)
        {
            return color switch
            {
                CategoryColor.Blue => "#0000FF",
                CategoryColor.Green => "#00FF00",
                CategoryColor.Yellow => "#FFFF00",
                CategoryColor.Orange => "#FFA500",
                CategoryColor.Red => "#FF0000",
                _ => "#AAAAAA"
            };
        }
    }
}
