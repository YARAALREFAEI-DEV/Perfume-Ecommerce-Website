namespace WebAppPerfumes01.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public string UserId { get; set; }
        public decimal Total;

        //public decimal TotalPrice => Product.FinalPrice * Quantity;
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal TotalSum
        {
            get
            {
                return (Price * Quantity) + (decimal)ShippingPrice;
            }
        }
        public decimal ShippingPrice { get; set; }
    }
}
