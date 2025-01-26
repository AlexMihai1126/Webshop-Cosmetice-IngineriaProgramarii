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

        public IndexModel(Proiect_ipContext context)

        {
            _context = context;
        }

        public List<Produs> ProduseSlideshow { get; set; } 

        public async Task<IActionResult> OnGetAsync()
        {
            // Filtrează produsele care au reducere diferită de 0
            var produseCuReducere = await _context.Produse
                .Include(p => p.Categorie)
                .Include(p => p.Brand)
                .Where(p => p.Reducere != 0) // Filtrează doar produsele cu reducere
                .ToListAsync();

            // Alege aleatoriu 28 de produse din lista filtrată
            ProduseSlideshow = produseCuReducere
                .OrderBy(x => Guid.NewGuid())
                .Take(28)
                .ToList();

            return Page();
        }
    }
}
