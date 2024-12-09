using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;

namespace Proiect_ip.Services.DataCache
{
    public class CategoriiPartialService(Proiect_ipContext context)
    {
        private readonly Proiect_ipContext _context = context;

        public async Task<List<(int Id, string NumeCateg)>> GetCategoriiAsync()
        {
            return await _context.CategoriiProduse
                                 .Select(c => new { c.Id, c.NumeCateg })
                                 .ToListAsync()
                                 .ContinueWith(task => task.Result.Select(c => (c.Id, c.NumeCateg)).ToList());
        }
    }
}
