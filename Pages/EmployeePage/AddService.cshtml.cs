using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProjektRazor.Database;
using WebProjektRazor.Models;
using System.Threading.Tasks;

namespace WebProjektRazor.Pages.EmployeePage
{
    public class AddServiceModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddServiceModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderService NewOrderService { get; set; } = new OrderService();

        public void OnGet()
        {
            // Dodam ladowanie viewModeli jak je stworze 
        }

        public async Task<IActionResult> OnPostAddOrderServiceAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            _context.OrderServices.Add(NewOrderService);
            await _context.SaveChangesAsync();

            return RedirectToPage("/EmployeePage/Services");
        }
    }
}
