using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Services
{
    public class PointsService
    {
        private readonly IMemoryCache _cache;
        private readonly Proiect_ipContext _context;

        public PointsService(IMemoryCache cache, Proiect_ipContext context)
        {
            _cache = cache;
            _context = context;
        }

        public async Task CacheUserPointsAsync(string userId)
        {
            var pctTotal = await _context.IstoricPuncte
                .Where(ip => ip.Proiect_ipUserID == userId)
                .SumAsync(ip => ip.Puncte);

            _cache.Set($"points_{userId}", pctTotal, TimeSpan.FromMinutes(30));
        }

        public async Task<int> GetPointsAsync(string userId)
        {
            if (!_cache.TryGetValue($"points_{userId}", out int pctTotal))
            {
                pctTotal = await _context.IstoricPuncte
                    .Where(ip => ip.Proiect_ipUserID == userId)
                    .SumAsync(ip => ip.Puncte);

                _cache.Set($"points_{userId}", pctTotal, TimeSpan.FromMinutes(30));
            }
            return pctTotal;
        }

        public async Task AddPointsAsync(string userId, int nrPuncteAdaugat)
        {
            var pctNoi = new IstoricPuncte
            {
                Proiect_ipUserID = userId,
                Puncte = nrPuncteAdaugat,
                DataAdaugare = DateTime.UtcNow
            };

            _context.IstoricPuncte.Add(pctNoi);
            await _context.SaveChangesAsync();

            await CacheUserPointsAsync(userId);
        }
    }

}
