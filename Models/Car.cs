using WebProjektRazor.Models.User;

namespace WebProjektRazor.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; } = null!;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public int Mileage { get; set; }
    }
}
