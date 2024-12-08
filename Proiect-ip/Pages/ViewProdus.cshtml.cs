using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services;

namespace Proiect_ip.Pages
{
    public class ViewProdusModel : PageModel
    {
        private readonly Proiect_ipContext _context;

        public ViewProdusModel(Proiect_ipContext context)
        {
            _context = context;
        }

        public Produs Produs { get; set; }
        public List<SelectListItem> Categorii { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Produs = _context.Produse
            .Include(p => p.Categorie)
            .Include(p => p.Brand) 
            .FirstOrDefault(p => p.IdProdus == id); 

            if (Produs == null)
            {
                return NotFound();
            }

            return Page();
        }

    }


}