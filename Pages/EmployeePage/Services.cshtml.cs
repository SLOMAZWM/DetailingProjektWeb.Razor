using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjektRazor.Database;
using WebProjektRazor.Models;

namespace WebProjektRazor.Pages.EmployeePage
{
    public class ServicesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ServicesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IList<OrderService> OrderServices { get; set; }

        [BindProperty]
        public OrderService NewOrderService { get; set; } = new OrderService();

        public SelectList StatusList { get; set; }

        public async Task OnGetAsync()
        {
            OrderServices = await _context.OrderServices
                .Include(os => os.Client)
                    .ThenInclude(c => c.User)
                .Include(os => os.Car)
                .Include(os => os.Services)
                .ToListAsync();

            StatusList = new SelectList(new List<string> { "Oczekuje", "W trakcie", "Zrealizowane" });
        }

        public async Task<IActionResult> OnPostAddOrderServiceAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            _context.OrderServices.Add(NewOrderService);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string status)
        {
            var orderService = await _context.OrderServices.FindAsync(id);
            if (orderService == null)
            {
                return NotFound();
            }

            orderService.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
