using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Models.DTO;

namespace Proiect_ip.Pages.Admin.Branduri
{
    public class CreateBranduriModel(IWebHostEnvironment environment, Proiect_ipContext context) : PageModel
    {
        private readonly IWebHostEnvironment environment = environment;
        private readonly Proiect_ipContext context = context;
        [BindProperty]
        public BrandDto Brand { get; set; }

        public void OnGet()
        {
            Brand = new BrandDto();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var brandNou = new Brand
            {
                NumeBrand = Brand.Nume,
            };
            await context.Branduri.AddAsync(brandNou);
            await context.SaveChangesAsync();
            return RedirectToPage("/Admin/Branduri/Overview");
        }

    }
}