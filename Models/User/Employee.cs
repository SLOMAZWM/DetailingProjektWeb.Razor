﻿using System.Collections.Generic;

namespace WebProjektRazor.Models.User
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Position { get; set; } = string.Empty;
        public virtual ICollection<Order> AssignedOrders { get; set; }

        public Employee()
        {
            AssignedOrders = new List<Order>();
        }
    }
}
