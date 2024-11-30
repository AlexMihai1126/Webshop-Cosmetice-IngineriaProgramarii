using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages.Admin.Categorii
{
    public class DeleteCategoriiModel : PageModel
    {
        private readonly Proiect_ipContext _context;
        public DeleteCategoriiModel(Proiect_ipContext context)
        {
            _context = context;
        }
        [BindProperty]
        public CategorieProdus CategorieProdus { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CategorieProdus = await _context.CategoriiProduse.FindAsync(id);
            if (CategorieProdus == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CategorieProdus = await _context.CategoriiProduse.FindAsync(id);
            if (CategorieProdus != null)
            {
               _context.CategoriiProduse.Remove(CategorieProdus);
               await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Admin/Categorii/Overview");
        }
    }
}