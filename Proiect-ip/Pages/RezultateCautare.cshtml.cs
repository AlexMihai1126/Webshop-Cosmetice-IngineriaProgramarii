using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proiect_ip.Models;
using Proiect_ip.Services;

namespace Proiect_ip.Pages
{
    public class RezultateCautareModel(ProductSearchService searchService) : PageModel
    {
        private readonly ProductSearchService _searchService = searchService;

        public List<Produs> Produse { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; } = 1;  
        public int ItemsPerPage { get; set; } = 8;
        public string SearchQuery { get; set; }

        public async Task<IActionResult> OnGetAsync(string? term, int pageIndex = 1)
        {
            if(term == null)
            {
                return RedirectToPage("/Index");
            }
            SearchQuery = term;
            Produse = await _searchService.SearchBrandOrNameAsync(term);

            PageIndex = pageIndex;

            TotalPages = (int)Math.Ceiling(Produse.Count / (double)ItemsPerPage);
            Produse = Produse.Skip((pageIndex - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();

            return Page();
        }
    }
}
