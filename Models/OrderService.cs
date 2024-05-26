using System;
using System.Collections.Generic;

namespace WebProjektRazor.Models
{
    public class OrderService : Order
    {
        public int OrderServiceId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual Car Car { get; set; }

        public OrderService()
        {
            Services = new List<Service>();
        }
    }
}
