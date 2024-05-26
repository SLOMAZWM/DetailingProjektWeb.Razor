namespace WebProjektRazor.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderProducts> OrderProducts { get; set; }

        public Product()
        {
            OrderProducts = new List<OrderProducts>();
        }
    }
}
