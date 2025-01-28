using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proiect_ip.Services;

namespace Proiect_ip.Areas.Identity.Pages.Account.Manage
{
    public class MyOrdersModel(UserManager<Proiect_ipUser> userManager, OrdersManagerService ordersManager) : PageModel
    {
        public List<Comanda> Orders { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["ActivePage"] = "MyOrders";
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Orders = await ordersManager.GetCustomerOrdersAsync(user.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            await ordersManager.CancelOrderAsync(orderId,currentUser.Id);
            TempData["SuccessMessage"] = "Comanda a fost anulata cu succes!";
            return RedirectToPage();
        }
    }
}
