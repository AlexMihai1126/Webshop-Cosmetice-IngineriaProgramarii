using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Services.DataCache;
using Proiect_ip.Models;
using Proiect_ip.Data;
using Microsoft.EntityFrameworkCore;

namespace Proiect_ip.Areas.Identity.Pages.Account.Manage
{
    public class ViewRewardsModel(Proiect_ipContext context, UserManager<Proiect_ipUser> userManager, PointsService pointsService) : PageModel
    {

        public int Points { get; set; }
        public string? Message { get; set; }

        public List<IstoricPuncte> istoricPuncte { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["ActivePage"] = "ViewRewards";
            var user = await userManager.GetUserAsync(User);
            
            if (user != null)
            {
                try
                {
                    Points = await pointsService.GetPointsAsync(user.Id);
                    istoricPuncte = await context.IstoricPuncte
                        .Where(i => i.Proiect_ipUserID == user.Id)
                        .ToListAsync();
                }
                catch (Exception)
                {
                    Message = "Eroare la incarcarea punctelor. Va rugam sa reincercati.";
                }
            }
        }
    }
}
