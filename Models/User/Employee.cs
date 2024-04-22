using System.Collections.ObjectModel;

namespace WebProjektRazor.Models.User
{
    public class Employee : User
    {
        public int EmployeeId { get; set; }
        public string Position { get; set; }
        public ObservableCollection<Order> AssignedOrders { get; set; }

        public Employee(int employeeId = 0, string position = "brak wyniku", ObservableCollection<Order> assignedOrders = null!, int userId = 0, string firstName = "brak wyniku", string lastName = "brak wyniku", string email = "brak wyniku", string password = "brak wyniku", string phoneNumber = "brak wyniku")
            : base(userId, firstName, lastName, email, password, phoneNumber, isEmployer: true)
        {
            EmployeeId = employeeId;
            Position = position;
            AssignedOrders = assignedOrders;
        }
    }
}
