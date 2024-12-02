using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_ip.Services
{
    public class ProductSearchService(Proiect_ipContext context)
    {
        private readonly Proiect_ipContext _context = context;

        public async Task<List<Produs>> SearchBrandOrNameAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            IQueryable<Produs> query = _context.Produse;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Nume.ToLower().Contains(searchTerm) || p.Brand.ToLower().Contains(searchTerm));
            }

            return await query.ToListAsync();
        }
    }
}
