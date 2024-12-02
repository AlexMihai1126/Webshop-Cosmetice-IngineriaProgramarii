using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Models.DTO;

namespace Proiect_ip.Pages.Admin.Produse
{
    public class EditProduseModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly Proiect_ipContext context;
        public List<SelectListItem> Categorii { get; set; }
        public List<SelectListItem> Branduri { get; set; }

        [BindProperty]
        public ProdusDto ProdusDto { get; set; } = new ProdusDto();
        public Produs Produs { get; set; } = new Produs();
        public EditProduseModel(IWebHostEnvironment environment, Proiect_ipContext context)
        {
            this.environment = environment;
            this.context = context;
            Categorii = context.CategoriiProduse.Select(CategoriiProduse => new SelectListItem { Text = CategoriiProduse.NumeCateg, Value = CategoriiProduse.Id.ToString() }).ToList();
            Categorii.Insert(0, new SelectListItem { Text = "Fara categorie", Value = "" });
            Branduri = context.Branduri
               .Select(b => new SelectListItem { Text = b.NumeBrand, Value = b.BrandId.ToString() })
               .ToList();
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Produse/Overview");
                return;
            }

            var produs = context.Produse.Find(id);
            if (produs == null)
            {
                Response.Redirect("/Admin/Produse/Overview");
                return;
            }
            ProdusDto.Nume = produs.Nume;
            ProdusDto.BrandId = produs.IdBrand;
            ProdusDto.Descriere = produs.Descriere;
            ProdusDto.Pret = produs.Pret;
            ProdusDto.Stoc = produs.Stoc;
            ProdusDto.CategorieId = produs.IdCategorie;
            Produs = produs;
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            if (id == null)
            {
                return RedirectToPage("/Admin/Produse/Overview");
            }

            var produs = context.Produse.Find(id);
            if (produs == null)
            {
                return RedirectToPage("/Admin/Produse/Overview");
            }

            byte[] imageBytes = null;

            if (ProdusDto.Image != null)
            {
                if (ProdusDto.Image.ContentType != "image/png" && ProdusDto.Image.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProdusDto.Image", "Se pot incarca doar imagini PNG sau JPEG.");
                }
                else
                    using (var memoryStream = new MemoryStream())
                    {
                        await ProdusDto.Image.CopyToAsync(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            produs.Nume = ProdusDto.Nume;
            produs.IdBrand = ProdusDto.BrandId;
            produs.Descriere = ProdusDto.Descriere;
            produs.Pret = ProdusDto.Pret;
            produs.Stoc = ProdusDto.Stoc;
            produs.IdCategorie = ProdusDto.CategorieId;
            produs.ImageData = imageBytes;
            produs.ImageType = ProdusDto.Image?.ContentType;
            await context.SaveChangesAsync();

            return RedirectToPage("/Admin/Produse/Overview");
        }
    }
}