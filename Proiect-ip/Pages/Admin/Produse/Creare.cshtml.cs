using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Proiect_ip.Models.DTO;
using Proiect_ip.Models;
using Proiect_ip.Data;

namespace Proiect_ip.Pages.Admin.Produse
{
    public class CreareProdusModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly Proiect_ipContext context;
        public List<SelectListItem> Categorii { get; set; }


        [BindProperty]
        public ProdusDto ProdusDto { get; set; }

        public CreareProdusModel(IWebHostEnvironment environment, Proiect_ipContext context)
        {
            this.environment = environment;
            this.context = context;
            Categorii = context.CategoriiProduse
               .Select(c => new SelectListItem { Text = c.NumeCateg, Value = c.Id.ToString() })
               .ToList();
            Categorii.Insert(0, new SelectListItem { Text = "Fara categorie", Value = "" });
        }

        public void OnGet()
        {
            ProdusDto = new ProdusDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
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

            Produs produs = new Produs
            {
                Nume = ProdusDto.Nume,
                Descriere = ProdusDto.Descriere,
                Pret = ProdusDto.Pret,
                Stoc = ProdusDto.Stoc,
                Brand = ProdusDto.Brand,
                IdCategorie = ProdusDto.CategorieId == 0 ? (int?)null : ProdusDto.CategorieId,
                ImageData = imageBytes != null ? imageBytes : null,
                ImageType = ProdusDto.Image?.ContentType
            };

            context.Produse.Add(produs);
            await context.SaveChangesAsync();
            return RedirectToPage("/Admin/Produse/Overview");
        }
    }
}
