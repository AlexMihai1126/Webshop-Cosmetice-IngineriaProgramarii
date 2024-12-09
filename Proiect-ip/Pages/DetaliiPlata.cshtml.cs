using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Areas.Identity.Data;

namespace Proiect_ip.Pages
{
    public class DetaliiPlataModel : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager;

        public DetaliiPlataModel(UserManager<Proiect_ipUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string Nume { get; set; }

        [BindProperty]
        public string Prenume { get; set; }

        [BindProperty]
        public string Telefon { get; set; }

        [BindProperty]
        public string Adresa { get; set; }

        [BindProperty]
        public string MetodaPlata { get; set; }

        [BindProperty]
        public decimal TotalDePlata { get; set; } = 100.00m; // Default total price for demo


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (MetodaPlata == "Numerar")
                {
                    return RedirectToPage("/ConfirmPlata", new { status = "Success" });
                }
                else if (MetodaPlata == "Card")
                {
                    // Simuleazã procesarea plã?ii prin card
                    return RedirectToPage("/ConfirmPlata", new { status = "Success" });
                }
            }

            return Page();
        }
    }
}
