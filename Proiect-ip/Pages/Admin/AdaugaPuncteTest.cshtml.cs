using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Services;

namespace Proiect_ip.Pages.Admin
{
    public class AdaugaPuncteTestModel : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager;
        private readonly PointsService _pointsService;

        public AdaugaPuncteTestModel(UserManager<Proiect_ipUser> userManager, PointsService pointsService)
        {
            _userManager = userManager;
            _pointsService = pointsService;
        }

        [BindProperty]
        public int Points { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            try
            {
                await _pointsService.ModifyPointsAsync(user.Id, Points);
            }
            catch(InvalidOperationException)
            {
                Message = "Punctele nu pot fi <0 !";
                return Page();
            }
            
            Message = $"Am modificat cu {Points} puncte contul!";
            return Page();
        }
    }
}
