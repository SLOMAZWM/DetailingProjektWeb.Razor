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
        public Client(int clientId = 0, ObservableCollection<OrderProducts> historyProductsOrders = null!, ObservableCollection<OrderService> historyServiceOrders = null!,
            int userId = 0, string firstName = "", string lastName = "", string email = "", string password = "", string phoneNumber = "")
            : base(userId, firstName, lastName, email, password, phoneNumber, isEmployer: true)
        {
            ClientId = clientId;
            HistoryProductsOrders = historyProductsOrders;
            HistoryServiceOrders = historyServiceOrders;
        }
    }
}
