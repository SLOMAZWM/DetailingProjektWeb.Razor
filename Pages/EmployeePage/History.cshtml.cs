using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjektRazor.Database;
using WebProjektRazor.Models;

namespace WebProjektRazor.Pages.EmployeePage
{
    public class HistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public HistoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<OrderProducts> OrderProducts { get; set; }
        public Dictionary<int, decimal> OrderTotals { get; set; } = new Dictionary<int, decimal>();

        public async Task OnGetAsync()
        {
            OrderProducts = await _context.Orders
                .OfType<OrderProducts>()
                .Include(o => o.Client)
                    .ThenInclude(c => c.User)
                .Include(o => o.Car)
                .Include(o => o.Products)
                .ToListAsync();

            foreach (var order in OrderProducts)
            {
                OrderTotals[order.OrderId] = order.Products.Sum(p => p.Price);
            }
        }
    }
}
