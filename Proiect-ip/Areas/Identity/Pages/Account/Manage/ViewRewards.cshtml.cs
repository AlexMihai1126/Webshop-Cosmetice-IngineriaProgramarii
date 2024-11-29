using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Proiect_ip.Areas.Identity.Data;

namespace Proiect_ip.Areas.Identity.Pages.Account.Manage
{
    public class ViewRewardsModel : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager;

        public ViewRewardsModel(UserManager<Proiect_ipUser> userManager)
        {
            _userManager = userManager;
        }

        public int? Points { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["ActivePage"] = "ViewRewards";
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Points = user.Puncte; // Assuming "Points" is a property in your `Proiect_ipUser` model
            }
        }
    }
}
