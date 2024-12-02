using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Models.DTO;

namespace Proiect_ip.Pages.Admin.Branduri
{
    public class EditBranduriModel(IWebHostEnvironment environment, Proiect_ipContext context) : PageModel
    {
        private readonly IWebHostEnvironment environment = environment;
        private readonly Proiect_ipContext context = context;
        [BindProperty]
        public BrandDto BrandDto { get; set; } = new BrandDto();
        public Brand BrandProdus { get; set; } = new Brand();

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Branduri/Overview");
                return;
            }
            var brand = context.Branduri.Find(id);
            if (brand == null)
            {
                Response.Redirect("/Admin/Branduri/Overview");
                return;
            }
            BrandDto.Nume = brand.NumeBrand;
            BrandProdus = brand;
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Admin/Branduri/Overview");
            }
            var brand = context.Branduri.Find(id);

            if (brand == null)
            {
                return RedirectToPage("/Admin/Branduri/Overview");
            }
            brand.NumeBrand = BrandDto.Nume;
            await context.SaveChangesAsync();
            return RedirectToPage("/Admin/Branduri/Overview");
        }
    }
}