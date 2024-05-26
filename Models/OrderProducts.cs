using System.Collections.Generic;

namespace WebProjektRazor.Models
{
    public class OrderProducts : Order
    {
        public int OrderProductsId { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public OrderProducts()
        {
            Products = new List<Product>();
        }
    }
}
