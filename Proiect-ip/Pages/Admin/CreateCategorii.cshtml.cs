using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Models.DTO;

namespace Proiect_ip.Pages.Admin
{
    public class CreateCategoriiModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly Proiect_ipContext context;
        [BindProperty]
        public CategorieDto Categorie { get; set; }
        public CreateCategoriiModel(IWebHostEnvironment environment, Proiect_ipContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet()
        {
            Categorie = new CategorieDto();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categNoua = new CategorieProdus
            {
                NumeCateg = Categorie.Nume,
                Descriere = Categorie.Descriere
            };
            await context.CategoriiProduse.AddAsync(categNoua);
            await context.SaveChangesAsync();
            return RedirectToPage("/Admin/Categorii");
        }

    }
}