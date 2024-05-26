using WebProjektRazor.Models.User;

namespace WebProjektRazor.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Client Client { get; set; }
    }
}
