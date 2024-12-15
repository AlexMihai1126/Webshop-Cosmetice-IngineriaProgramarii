using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Services;

namespace Proiect_ip.Pages
{
    public class DetaliiPlataModel : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager;
        private readonly ShoppingCartService _cartService;

        public DetaliiPlataModel(UserManager<Proiect_ipUser> userManager, ShoppingCartService cartService)
        {
            _userManager = userManager;
            _cartService = cartService; 
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
        public decimal TotalDePlata { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (!string.IsNullOrEmpty(userId))
            {
                // metoda din ShoppingCartService pentru a obtine totalul
                var (suma, _) = await _cartService.GetCartDataAsync(userId);
                TotalDePlata = suma;
            }
            else
            {
                TotalDePlata = 0; // Daca utilizatorul nu este conectat sau cosul este gol
            }
        }


        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                await _cartService.PlaceOrderAsync(userId);

                return RedirectToPage("/ConfirmPlata", new { status = "Success" });
            }

            return Page();
        }


    }
}