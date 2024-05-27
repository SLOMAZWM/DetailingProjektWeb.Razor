using System.Collections.Generic;

namespace WebProjektRazor.Models
{
    public class OrderProducts : Order
    {
        public string Status { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
