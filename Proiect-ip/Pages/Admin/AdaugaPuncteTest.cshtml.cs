using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Services.DataCache;

namespace Proiect_ip.Pages.Admin
{
    public class AdaugaPuncteTestModel(UserManager<Proiect_ipUser> userManager, PointsService pointsService) : PageModel
    {
        private readonly UserManager<Proiect_ipUser> _userManager = userManager;
        private readonly PointsService _pointsService = pointsService;

        [BindProperty]
        public int PctDeModificat { get; set; }
        [BindProperty]
        public string? Motiv { get; set; }
        [BindProperty]
        public int? IdCom { get; set; }

        public string? Message { get; set; }

        private int PctModif {  get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            try
            {
                PctModif = await _pointsService.ModifyPointsAsync(user.Id, PctDeModificat, Motiv, IdCom);
            }
            catch(InvalidOperationException)
            {
                Message = "Punctele nu pot fi <0 !";
                return Page();
            }
            
            Message = $"Am modificat cu {PctModif} puncte contul!";
            return Page();
        }
    }
}
