namespace Budget.Server.Core.Categories.Enums
{
    public enum CategoryIcon
    {
        None = 0,
        Bank = 1,
        Bill = 2,
        House = 3,
        Wallet = 4,
        CreditCard = 5,
        Coins = 6,
        PiggyBank = 7,
        Cart = 8,
        Gift = 9,
        Car = 10,
        Plane = 11,
        Restaurant = 12,
    }

    public static class CategoryIconExtension
    {
        public static string ToClassName(this CategoryIcon icon)
        {
            return icon switch
            {
                CategoryIcon.Bank => "bank",
                CategoryIcon.Bill => "bill",
                CategoryIcon.House => "house",
                CategoryIcon.Wallet => "wallet",
                CategoryIcon.CreditCard => "credit-card",
                CategoryIcon.Coins => "coins",
                CategoryIcon.PiggyBank => "piggy-bank",
                CategoryIcon.Cart => "cart",
                CategoryIcon.Gift => "gift",
                CategoryIcon.Car => "car",
                CategoryIcon.Plane => "plane",
                CategoryIcon.Restaurant => "restaurant",
                _ => string.Empty,
            };
        }
    }
}
