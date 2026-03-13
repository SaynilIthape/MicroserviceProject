namespace Basket.API.Models
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();
        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * item.Qauntity;
                }
                return total;
            }
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }   


    }
}
