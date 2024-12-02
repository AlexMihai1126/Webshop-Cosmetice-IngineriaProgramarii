using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages.Admin.Branduri
{
    public class DeleteBranduriModel(Proiect_ipContext context) : PageModel
    {
        private readonly Proiect_ipContext _context = context;

        [BindProperty]
        public Brand BrandProdus { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            BrandProdus = await _context.Branduri.FindAsync(id);
            if (BrandProdus == null)
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
            BrandProdus = await _context.Branduri.FindAsync(id);
            if (BrandProdus != null)
            {
               _context.Branduri.Remove(BrandProdus);
               await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Admin/Branduri/Overview");
        }
    }
}