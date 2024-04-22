namespace DetailingProjekt.Shared.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Service(int serviceId = 0, string name = "brak wyniku", string description = "brak wyniku", decimal price = 0)
        {
            ServiceId = serviceId;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
