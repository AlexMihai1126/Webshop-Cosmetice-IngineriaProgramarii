using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;


namespace Proiect_ip.Pages.Admin.Produse
{
    public class DeleteProduseModel : PageModel
    {
        private readonly Proiect_ipContext _context;

        public DeleteProduseModel(Proiect_ipContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Produs Produs { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Produs = _context.Produse.Find(id);

            if (Produs == null)
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

            Produs = _context.Produse.Find(id);

            if (Produs != null)
            {
                _context.Produse.Remove(Produs);
                _context.SaveChanges();
            }

            return RedirectToPage("/Admin/Produse/Overview");
        }
    }
}
