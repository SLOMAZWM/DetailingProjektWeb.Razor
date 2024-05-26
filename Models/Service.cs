namespace WebProjektRazor.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderService> OrderServices { get; set; }

        public Service()
        {
            OrderServices = new List<OrderService>();
        }
    }
}
