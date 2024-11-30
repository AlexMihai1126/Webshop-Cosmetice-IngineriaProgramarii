using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Services;

namespace Proiect_ip.Areas.Identity.Pages.Account.Manage
{
    public class ViewRewardsModel : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager;
        private readonly PointsService _pointsService;

        public int Points { get; set; }

        public ViewRewardsModel(UserManager<Proiect_ipUser> userManager, PointsService pointsService)
        {
            _userManager = userManager;
            _pointsService = pointsService;
        }

        public async Task OnGetAsync()
        {
            ViewData["ActivePage"] = "ViewRewards";
            var user = await _userManager.GetUserAsync(User);
            
            if (user != null)
            {
               Points = await _pointsService.GetPointsAsync(user.Id);
            }
        }
    }
}
