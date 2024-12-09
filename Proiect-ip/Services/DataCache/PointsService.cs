using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Proiect_ip.Data;
using Proiect_ip.Models;

namespace Proiect_ip.Services.DataCache
{
    public class PointsService
    {
        private readonly IMemoryCache _cache;
        private readonly Proiect_ipContext _context;
        private readonly int POINTS_CAP = 100000;

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

        public async Task<int> ModifyPointsAsync(string userId, int nrPuncteAdaugat)
        {
            int nrPctCurent = await GetPointsAsync(userId);

            if (nrPctCurent + nrPuncteAdaugat < 0)
            {
                throw new InvalidOperationException("Cannot deduct points. The operation would result in negative points.");
            }

            nrPuncteAdaugat = nrPuncteAdaugat > POINTS_CAP ? POINTS_CAP : nrPuncteAdaugat;

            var pctNoi = new IstoricPuncte
            {
                Proiect_ipUserID = userId,
                Puncte = nrPuncteAdaugat,
                DataAdaugare = DateTime.UtcNow
            };

            _context.IstoricPuncte.Add(pctNoi);
            await _context.SaveChangesAsync();
            await CacheUserPointsAsync(userId);

            return nrPuncteAdaugat;
        }
    }

}
