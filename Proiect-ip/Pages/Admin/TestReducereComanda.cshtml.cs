using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Models;
using Proiect_ip.Services;

namespace Proiect_ip.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class TestReducereComandaModel(OrdersManagerService ordersManager, UserManager<Proiect_ipUser> userManager) : PageModel
    {
        public List<Comanda> Orders { get; set; } = [];
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            Orders = await ordersManager.GetCustomerOrdersAsync(user.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            await ordersManager.UpdateOrderStatusAsync(orderId, Comanda.ComandaStatus.Anulat);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostConfirmOrderAsync(int orderId)
        {
            await ordersManager.UpdateOrderStatusAsync(orderId, Comanda.ComandaStatus.Confirmata);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostSendOrderAsync(int orderId)
        {
            await ordersManager.UpdateOrderStatusAsync(orderId, Comanda.ComandaStatus.Expediata);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostAddDiscountAsync(int orderId)
        {
            var user = await userManager.GetUserAsync(User);
            try
            {
                await ordersManager.AddDiscountAsync(orderId, user.Id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage();
            }
            TempData["SuccessMessage"] = "Discount aplicat cu succes!";
            return RedirectToPage();
        }
    }
}
