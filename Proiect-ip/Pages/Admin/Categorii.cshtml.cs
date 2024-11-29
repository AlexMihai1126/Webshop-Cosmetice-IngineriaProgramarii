using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages.Admin
{
    public class CategoriiModel : PageModel
    {
        private readonly Proiect_ipContext context;
        public List<CategorieProdus> Categorii { get; set; } = new List<CategorieProdus>();
        public CategoriiModel(Proiect_ipContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {
            Categorii = context.CategoriiProduse
                               .OrderByDescending(p => p.Id)
                               .ToList();
        }
    }
}