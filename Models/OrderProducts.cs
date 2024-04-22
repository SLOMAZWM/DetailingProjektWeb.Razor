using System.Collections.ObjectModel;

namespace DetailingProjekt.Shared.Models
{
    public class OrderProducts : Order
    {
        public int OrderProductsId { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }

        public OrderProducts(int orderProductsId = 0, ObservableCollection<Product> products = null!, string status = "brak wyniku", string address = "brak wyniku",
            int orderId = 0, Employee employeer = null!, Client client = null!) : base(orderId, employeer, client) 
        {
            OrderProductsId = orderProductsId;
            Products = products;
            Status = status;
            Address = address;
        }
    }
}
