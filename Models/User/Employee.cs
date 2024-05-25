using System.Collections.ObjectModel;

namespace WebProjektRazor.Models.User
{
    public class Employee : User
    {
        public int EmployeeId { get; set; }
        public string Position { get; set; } = string.Empty;
        public ObservableCollection<Order> AssignedOrders { get; set; }

        public Employee () : base ()
        {
            AssignedOrders = new ObservableCollection<Order>();
        }
    }
}
