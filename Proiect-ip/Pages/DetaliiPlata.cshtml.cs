using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Versioning;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Services;
using Proiect_ip.Services.DataCache;

namespace Proiect_ip.Pages
{
    [Authorize]
    public class DetaliiPlataModel : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager;
        private readonly ShoppingCartService _cartService;
        private readonly PointsService _pointsService;
        private readonly OrdersManagerService _ordersManager;

        public DetaliiPlataModel(
            UserManager<Proiect_ipUser> userManager,
            ShoppingCartService cartService,
            PointsService pointsService,
            OrdersManagerService ordersManager)
        {
            _userManager = userManager;
            _cartService = cartService;
            _pointsService = pointsService;
            _ordersManager = ordersManager;
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
        [BindProperty]
        public bool AplicaReducere { get; set; }

        public int UserPoints { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var (suma, nrProduse) = await _cartService.GetCartDataAsync(user.Id);
            if(suma == 0 && nrProduse == 0)
            {
                return RedirectToPage("/Cos");
            }
            UserPoints = await _pointsService.GetPointsAsync(user.Id);
            TotalDePlata = suma;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(User);
            int idComanda = 0;

            try
            {
                idComanda = await _cartService.PlaceOrder2Async(user.Id);
            }
            catch (Exception)
            {
                return RedirectToPage("/Cos");
            }

            if (AplicaReducere)
            {
                await _ordersManager.AddDiscountAsync(idComanda, user.Id);
            }

            return RedirectToPage("/ConfirmPlata", new { status = "Success" });

        }
    }
}