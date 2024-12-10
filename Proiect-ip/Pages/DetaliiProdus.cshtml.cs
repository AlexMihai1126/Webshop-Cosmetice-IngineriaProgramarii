using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Proiect_ip.Pages
{
    public class ViewProdusModel(Proiect_ipContext context, ShoppingCartService cartService, UserManager<Proiect_ipUser> userManager) : PageModel
    {
        private readonly Proiect_ipContext _context = context;
        private readonly ShoppingCartService _cartService = cartService;
        private readonly UserManager<Proiect_ipUser> _userManager = userManager;

        public Produs? Produs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Produs = await _context.Produse
                .Include(p => p.Categorie)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.IdProdus == id);

            if (Produs == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAddToCartAsync(int id, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Redirect to the login page and include the return URL
                var returnUrl = Url.Page("DetaliiProdus", new { id });
                return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = returnUrl });
            }

            try
            {
                // Add the product to the cart
                await _cartService.AddToCartAsync(user.Id, id, quantity);

                // Redirect to the shopping cart page after successful addition
                TempData["SuccessMessage"] = "Produsul a fost adăugat în coș!";
                return RedirectToPage("/Cos");
            }
            catch (InvalidOperationException)
            {
                // Handle the case where adding to the cart fails (e.g., insufficient stock)
                TempData["ErrorMessage"] = "Produsul nu poate fi adăugat în coș!";
                return RedirectToPage("DetaliiProdus", new { id });
            }
        }

    }
}
