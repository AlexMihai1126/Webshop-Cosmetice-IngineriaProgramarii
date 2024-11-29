using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Models.DTO;

namespace Proiect_ip.Pages.Admin
{
    public class EditCategoriiModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly Proiect_ipContext context;
        [BindProperty]
        public CategorieDto CategorieDto { get; set; } = new CategorieDto();
        public CategorieProdus CategorieProdus { get; set; } = new CategorieProdus();
        public EditCategoriiModel(IWebHostEnvironment environment, Proiect_ipContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Categorii");
                return;
            }
            var categ = context.CategoriiProduse.Find(id);
            if (categ == null)
            {
                Response.Redirect("/Admin/Categorii");
                return;
            }
            CategorieDto.Nume = categ.NumeCateg;
            CategorieDto.Descriere = categ.Descriere;
            CategorieProdus = categ;
        }
        public void OnPost(int? id)
        {
            if (!ModelState.IsValid)
            {
                Response.Redirect("/Admin/Categorii");
                return;
            }
            var categ = context.CategoriiProduse.Find(id);
            if (categ == null)
            {
                Response.Redirect("/Admin/Categorii");
                return;
            }
            categ.NumeCateg = CategorieDto.Nume;
            categ.Descriere = CategorieDto.Descriere;
            context.SaveChanges();
            Response.Redirect("/Admin/Categorii");
        }
    }
}