namespace WebProjektRazor.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Employee Employee { get; set; }
        public Client Client { get; set; }

        public Order(int orderId = 0, Employee employeer = null!, Client client = null!)
        {
            OrderId = orderId;
            Employee = employeer;
            Client = client;
        }
    }
}
