using System;
using System.Collections.Generic;

namespace WebProjektRazor.Models
{
    public class OrderService : Order
    {
        public DateTime ExecutionDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
