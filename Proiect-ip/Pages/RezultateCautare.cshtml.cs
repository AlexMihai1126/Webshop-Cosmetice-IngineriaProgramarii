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
        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public async Task OnGetAsync()
        {
            Produse = await _searchService.SearchBrandOrNameAsync(SearchQuery);
        }
    }
}
