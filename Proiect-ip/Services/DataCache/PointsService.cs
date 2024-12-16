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

        public async Task<int> ModifyPointsAsync(string userId, int nrPuncteModificat, string? motiv, int? IdComanda)
        {
            int nrPctCurent = await GetPointsAsync(userId);
            if(nrPuncteModificat == 0)
            {
                throw new InvalidOperationException("Cannot add 0 points.");
            }

            if (nrPctCurent + nrPuncteModificat < 0)
            {
                throw new InvalidOperationException("Cannot deduct points. The operation would result in negative points.");
            }

            nrPuncteModificat = nrPuncteModificat > POINTS_CAP ? POINTS_CAP : nrPuncteModificat;

            var pctNoi = new IstoricPuncte
            {
                Proiect_ipUserID = userId,
                Puncte = nrPuncteModificat,
                DataAdaugare = DateTime.UtcNow,
                Motiv = motiv,
                IdComanda = IdComanda
            };

            _context.IstoricPuncte.Add(pctNoi);
            await _context.SaveChangesAsync();
            await CacheUserPointsAsync(userId);

            return nrPuncteModificat;
        }
    }
    //TODO - punctele ii vin user-ului numai dupa ce se confirma comanda de admin

}
