using System.Collections.ObjectModel;
using WebProjektRazor.Models.User;


namespace WebProjektRazor.Models
{
    public class OrderService : Order
    {
        public int OrderServiceId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string Status { get; set; }
        public ObservableCollection<Service> Services { get; set; }

        public OrderService(int orderServiceId = 0, DateTime executionDate = default, string status = "brak wyniku",
            ObservableCollection<Service> services = null!, int orderId = 0, Employee employeer = null!,
            Client client = null!) : base(orderId, employeer, client)
        {
            OrderServiceId = orderServiceId;
            ExecutionDate = executionDate;
            Status = status;
            Services = services;
        }
    }
}
