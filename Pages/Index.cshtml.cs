using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProjektRazor.Database;
using WebProjektRazor.Models;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> Products { get; set; }

    public void OnGet()
    {
        Products = _context.Products.ToList();
    }
}
