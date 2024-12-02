using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Pages.Admin.Branduri
{
    public class BranduriModel(Proiect_ipContext context) : PageModel
    {
        private readonly Proiect_ipContext context = context;
        public List<Brand> Branduri { get; set; } = new List<Brand>();

        public void OnGet()
        {
            Branduri = context.Branduri
                               .OrderByDescending(p => p.BrandId)
                               .ToList();
        }
    }
}