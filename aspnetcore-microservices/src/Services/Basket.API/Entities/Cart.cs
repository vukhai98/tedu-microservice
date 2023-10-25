namespace Basket.API.Entities
{
    public class Cart
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public Cart()
        {
        }

        public Cart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice => Items.Sum(x => x.ItemPrice * x.Quantity);

        public DateTimeOffset LastModifiedDate { get; set; } = DateTime.UtcNow;
        public string? JobId { get; set; } 
    }
}
