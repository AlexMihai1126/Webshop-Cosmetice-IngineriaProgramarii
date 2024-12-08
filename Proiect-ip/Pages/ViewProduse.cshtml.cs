using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services;


namespace Proiect_ip.Pages
{
    public class ViewProduseModel : PageModel
    {
        private readonly Proiect_ipContext _context;
        private readonly ShoppingCartService _shoppingCartService;


        public IList<Produs> Produse { get; set; }
        public int PageSize { get; } = 8; // Numărul de produse pe pagină
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        // Proprietate pentru a calcula numărul total de pagini
        public int TotalPages { get; set; }


        [BindProperty(SupportsGet = true)]
        public string Categorie { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Brand { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Nume { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? PretMin { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? PretMax { get; set; }

        public ViewProduseModel(Proiect_ipContext context, ShoppingCartService shoppingCartService)
        {
            _context = context;
            _shoppingCartService = shoppingCartService;
        }

        public IList<Produs> Produs { get; set; }

        public async Task OnGetAsync()
        {


            IQueryable<Produs> query = _context.Produse.Include(p => p.Categorie).Include(p => p.Brand);


            if (!string.IsNullOrEmpty(Categorie))
            {
                string categorieLower = Categorie.ToLower();
                query = query.Where(p => p.Categorie.NumeCateg.ToLower() == categorieLower);
            }


            if (!string.IsNullOrEmpty(Brand))
            {
                string brandLower = Brand.ToLower();
                query = query.Where(p => p.Brand.NumeBrand.ToLower() == brandLower);
            }

            if (!string.IsNullOrEmpty(Nume))
            {
                string modelLower = Nume.ToLower();
                query = query.Where(p => p.Nume.ToLower().Contains(modelLower));
            }

            if (PretMin.HasValue)
            {
                query = query.Where(p => p.Pret >= PretMin);
            }
            if (PretMax.HasValue)
            {
                query = query.Where(p => p.Pret <= PretMax);
            }

            // Numărul total de produse după filtrare
            int totalItems = await query.CountAsync();

            // Calcularea numărului total de pagini
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            // Obținerea paginii curente și a listei de produse
            Produs = await query.Skip((PageIndex - 1) * PageSize)
                               .Take(PageSize)
                               .ToListAsync();
        }


        public async Task<IActionResult> OnPostResetFilters()
        {
            Brand = null;
            Nume = null;
            PretMin = null;
            PretMax = null;

            return RedirectToPage(new { Categorie });
        }
    }
}