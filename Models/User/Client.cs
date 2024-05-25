using System.Collections.ObjectModel;

namespace WebProjektRazor.Models.User
{
    public class Client : User
    {
        public int ClientId { get; set; }
        public ObservableCollection<OrderProducts> HistoryProductsOrders { get; set; }
        public ObservableCollection<OrderService> HistoryServiceOrders { get; set; }

        public Client()
        {
            HistoryProductsOrders = new ObservableCollection<OrderProducts>();
            HistoryServiceOrders = new ObservableCollection<OrderService>();
        }
    }
}
