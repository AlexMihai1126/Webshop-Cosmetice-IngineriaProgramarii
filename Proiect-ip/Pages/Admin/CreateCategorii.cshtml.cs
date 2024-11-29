using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages.Admin
{
    public class CreateCategoriiModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly Proiect_ipContext context;
        public List<SelectListItem> Categorii { get; set; }
        [BindProperty]
        public CategorieProdus Categorie { get; set; }
        public CreateCategoriiModel(IWebHostEnvironment environment, Proiect_ipContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet()
        {
            Categorie = new CategorieProdus();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Console.WriteLine($"Salvare categorie: {Categorie.NumeCateg}");
            context.CategoriiProduse.Add(Categorie);
            context.SaveChanges();
            return RedirectToPage("/Admin/Categorii");
        }
    }
}