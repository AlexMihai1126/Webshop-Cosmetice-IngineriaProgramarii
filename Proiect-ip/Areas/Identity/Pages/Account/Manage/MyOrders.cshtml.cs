using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_ip.Areas.Identity.Pages.Account.Manage
{
    public class MyOrdersModel : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager;
        private readonly Proiect_ipContext _context;

        public MyOrdersModel(UserManager<Proiect_ipUser> userManager, Proiect_ipContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<Comanda> Orders { get; set; } = new List<Comanda>();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["ActivePage"] = "MyOrders";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Orders = _context.Comenzi
                .Where(c => c.Proiect_ipUserID == user.Id)
                .OrderByDescending(c => c.DataComanda)
                .ToList();

            return Page();
        }
    }
}
