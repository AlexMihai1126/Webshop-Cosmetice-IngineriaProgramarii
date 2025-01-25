using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services;

namespace Proiect_ip.Pages
{
    [Authorize]
    public class CosModel(UserManager<Proiect_ipUser> userManager, ShoppingCartService cartService, Proiect_ipContext context) : PageModel
    {
        public List<(int ProductId, int Cantitate)> Produse { get; set; }
        public List<Produs> ProduseCos { get; set; } = [];
        public decimal TotalCos { get; set; }
        public int TotalProduse { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            var userId = user.Id;
            var (suma, nrProd) = await cartService.GetCartDataAsync(user.Id);
            TotalCos = suma;
            TotalProduse = nrProd;
            Produse = await cartService.GetProductsAsync(userId);
            foreach (var product in Produse)
            {
                var produsNou = await context.Produse
                 .Include(p => p.Categorie)
                 .Include(p => p.Brand)
                 .FirstOrDefaultAsync(p => p.IdProdus == product.ProductId);

                ProduseCos.Add(produsNou);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromCartAsync(int productId)
        {
            var user = await userManager.GetUserAsync(User);
            var userId = user.Id;
            await cartService.RemoveFromCartAsync(userId, productId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateOrderAsync()
        {
            var user = await userManager.GetUserAsync(User);
            var userId = user.Id;
            //idee todo - serviciul creeaza comanda si returneaza dupa id-ul ei
            return RedirectToPage("DetaliiPlata", new { userId });
        }
        public async Task<IActionResult> OnPostUpdateQuantityAsync(int productId, int cantitate)
        {
            if (cantitate <= 0)
            {
                return Page();
            }
            var user = await userManager.GetUserAsync(User);
            try
            {
                await cartService.UpdateCartItemAsync(user.Id, productId, cantitate);
                TempData["SuccessMessage"] = "Am actualizat cantitatea.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Eroare la actualizare cantitate: {ex.Message}";
            }

            return RedirectToPage();
        }


    }

}