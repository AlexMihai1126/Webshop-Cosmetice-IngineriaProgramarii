using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services;

namespace Proiect_ip.Pages
{
    public class ProduseModel(Proiect_ipContext context) : PageModel
    {
        private readonly Proiect_ipContext _context = context;
        [BindProperty(SupportsGet = true)]
        public int ItemsPerPage { get; } = 8;
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategorieId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? BrandId { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? PretMin { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? PretMax { get; set; }
        public IList<Produs> Produse { get; set; }
        public IList<Brand> Branduri { get; set; }
        public IList<CategorieProdus> Categorii { get; set; }
        public List<SelectListItem> BranduriSelectList { get; set; }
        public List<SelectListItem> CategoriiSelectList { get; set; }
        public string? NumeCategCurenta { get; set; } = null;
        public string? NumeBrandCurent { get; set; } = null;
        public decimal PretMaximDB { get; set; }
        public decimal PretMinimDB { get; set; }
        public async Task OnGetAsync()
        {
            PretMaximDB = await _context.Produse.MaxAsync(p => p.Pret);
            PretMinimDB = await _context.Produse.MinAsync(p => p.Pret);
            Branduri = await _context.Branduri.OrderBy(b => b.NumeBrand).ToListAsync();
            Categorii = await _context.CategoriiProduse.OrderBy(c => c.NumeCateg).ToListAsync();

            BranduriSelectList = Branduri
                .Select(b => new SelectListItem
                {
                    Value = b.BrandId.ToString(),
                    Text = b.NumeBrand,
                    Selected = b.BrandId == BrandId
                })
                .ToList();
            BranduriSelectList.Insert(0, new SelectListItem { Text = "Niciun brand selectat", Value = "0" });

            CategoriiSelectList = Categorii
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.NumeCateg,
                    Selected = c.Id == CategorieId
                })
                .ToList();
            CategoriiSelectList.Insert(0, new SelectListItem { Text = "Nicio categorie selectata", Value = "0" });

            IQueryable<Produs> query = _context.Produse.Include(p => p.Categorie).Include(p => p.Brand);

            if (CategorieId.HasValue && CategorieId.Value != 0)
            {
                query = query.Where(p => p.IdCategorie == CategorieId.Value);
                var categorie = await _context.CategoriiProduse.FindAsync(CategorieId);
                NumeCategCurenta = categorie?.NumeCateg;
            }

            if (BrandId.HasValue && BrandId.Value != 0)
            {
                query = query.Where(p => p.IdBrand == BrandId.Value);
                var brand = await _context.Branduri.FindAsync(BrandId);
                NumeBrandCurent = brand?.NumeBrand;
            }

            if (PretMin.HasValue)
            {
                query = query.Where(p => p.Pret >= PretMin.Value);
            }

            if (PretMax.HasValue)
            {
                query = query.Where(p => p.Pret <= PretMax.Value);
            }

            int totalItems = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)ItemsPerPage);
            Produse = await query.Skip((PageIndex - 1) * ItemsPerPage)
                                   .Take(ItemsPerPage)
                                   .ToListAsync();
        }

        public Task<IActionResult> OnPostResetFilters()
        {
            CategorieId = 0;
            BrandId = 0;
            PretMin = null;
            PretMax = null;

            return Task.FromResult<IActionResult>(RedirectToPage());
        }
    }

}