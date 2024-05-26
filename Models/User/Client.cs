using System.Collections.Generic;

namespace WebProjektRazor.Models.User
{
    public class Client : User
    {
        public virtual ICollection<OrderProducts> HistoryProductsOrders { get; set; }
        public virtual ICollection<OrderService> HistoryServiceOrders { get; set; }

        public Client()
        {
            HistoryProductsOrders = new List<OrderProducts>();
            HistoryServiceOrders = new List<OrderService>();
        }
    }
}
