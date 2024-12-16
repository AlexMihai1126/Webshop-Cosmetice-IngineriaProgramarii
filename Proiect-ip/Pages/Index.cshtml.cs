using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; 
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Proiect_ipContext _context; 

        public IndexModel(ILogger<IndexModel> logger, Proiect_ipContext context)

        {
            _logger = logger;
            _context = context;
        }

        public List<Produs> ProduseSlideshow { get; set; } 

        public async Task<IActionResult> OnGetAsync()
        {
            ProduseSlideshow = await _context.Produse
        .Include(p => p.Categorie)
        .Include(p => p.Brand)
        .OrderBy(p => p.IdProdus)
        .Take(20)
        .ToListAsync();

            return Page();
        }
    }
}
