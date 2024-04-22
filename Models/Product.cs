namespace DetailingProjekt.Shared.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }

        public Product(int productId = 0, string name = "brak wyniku", short quantity = 0, decimal price = 0)
        {
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
}
