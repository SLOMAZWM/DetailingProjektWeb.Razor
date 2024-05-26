using System.Collections.Generic;

namespace WebProjektRazor.Models.User
{
    public class Employee : User
    {
        public string Position { get; set; } = string.Empty;
        public virtual ICollection<Order> AssignedOrders { get; set; }

        public Employee()
        {
            AssignedOrders = new List<Order>();
        }
    }
}
