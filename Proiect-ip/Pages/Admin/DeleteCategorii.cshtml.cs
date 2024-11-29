using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages.Admin
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
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CategorieProdus = _context.CategoriiProduse.Find(id);
            if (CategorieProdus == null)
            {
                return NotFound();
            }
            return Page();
        }
        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CategorieProdus = _context.CategoriiProduse.Find(id);
            if (CategorieProdus != null)
            {
                _context.CategoriiProduse.Remove(CategorieProdus);
                _context.SaveChanges();
            }
            return RedirectToPage("/Admin/Categorii");
        }
    }
}