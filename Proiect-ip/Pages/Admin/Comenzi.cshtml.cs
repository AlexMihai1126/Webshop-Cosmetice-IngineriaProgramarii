using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services;

namespace Proiect_ip.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ComenziModel(UserManager<Proiect_ipUser> userManager, OrdersManagerService ordersManager, Proiect_ipContext context) : PageModel
    {
        public List<Comanda> Orders { get; set; } = [];
        [BindProperty]
        public bool sortByLatest { get; set; }
        [BindProperty]
        public Comanda.ComandaStatus StatusFilter { get; set; }
        public async Task<IActionResult> OnGetAsync(Comanda.ComandaStatus? statusFilter, bool isLatest = true)
        {
            StatusFilter = statusFilter ?? Comanda.ComandaStatus.Toate;
            sortByLatest = isLatest;

            Orders = await ordersManager.GetAllOrdersAsync(StatusFilter, sortByLatest);
            return Page();
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            await ordersManager.UpdateOrderStatusAsync(orderId,Comanda.ComandaStatus.Anulat);
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
    }
}
