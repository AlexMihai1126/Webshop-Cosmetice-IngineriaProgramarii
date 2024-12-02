using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages.Admin.Produse
{
    public class ProduseModel : PageModel
    {
        private readonly Proiect_ipContext context;

        public List<Produs> Produs { get; set; } = new List<Produs>();

        public ProduseModel(Proiect_ipContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {
            Produs = context.Produse
                        .Include(p => p.Categorie)
                        .Include(p => p.Brand)
                        .OrderByDescending(p => p.IdProdus)
                        .ToList();
        }
    }
}