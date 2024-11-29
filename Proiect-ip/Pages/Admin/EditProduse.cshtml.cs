using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Models.DTO;

namespace Proiect_ip.Pages.Admin
{
    public class EditProduseModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly Proiect_ipContext context;
        public List<SelectListItem> Categorii { get; set; }


        [BindProperty]
        public ProdusDto ProdusDto { get; set; } = new ProdusDto();
        public Produs Produs { get; set; } = new Produs();
        public EditProduseModel(IWebHostEnvironment environment, Proiect_ipContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Produse");
                return;
            }

            var produs = context.Produse.Find(id);
            if (produs == null)
            {
                Response.Redirect("/Admin/Produse");
                return;
            }
            Categorii = context.CategoriiProduse.Select(CategoriiProduse => new SelectListItem { Text = CategoriiProduse.NumeCateg, Value = CategoriiProduse.Id.ToString() }).ToList();
            ProdusDto.Nume = produs.Nume;
            ProdusDto.Descriere = produs.Descriere;


            ProdusDto.Pret = produs.Pret;

            ProdusDto.Stoc = produs.Stoc;
            ProdusDto.CategorieId = (int)produs.IdCategorie;
            Produs = produs;
        }

        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Produse");
                return;
            }

            var produs = context.Produse.Find(id);
            if (produs == null)
            {
                Response.Redirect("/Admin/Produse");
                return;
            }

            // update produs in baza de date 
            produs.Nume = ProdusDto.Nume;

            produs.Descriere = ProdusDto.Descriere;

            produs.Pret = ProdusDto.Pret;

            produs.Stoc = ProdusDto.Stoc;
            produs.IdCategorie = ProdusDto.CategorieId;

            context.SaveChanges();

            Response.Redirect("/Admin/Produse");
        }
    }
}