using System.Collections.Generic;

namespace WebProjektRazor.Models.User
{
    public class Client
    {
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
