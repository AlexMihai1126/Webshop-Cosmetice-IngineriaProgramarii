using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Models.DataExtensions;
using Proiect_ip.Services.DataCache;

namespace Proiect_ip.Services
{
    public class ShoppingCartService(Proiect_ipContext context, PointsService pointsService, IMemoryCache memoryCache)
    {
        private readonly Proiect_ipContext _context = context;
        private readonly PointsService _pointsService = pointsService;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public class ProdusInCos
        {
            public int IdProdusCos { get; set; }
            public int CantitateInCos { get; set; }
            public decimal PretPerUnitate { get; set; }
        }
        public async Task<List<ProdusInCos>?> GetCartFromMemoryAsync(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            return await Task.FromResult(
                _memoryCache.TryGetValue(userId, out object? cosUtilizator) && cosUtilizator is List<ProdusInCos> listaProduse
                    ? listaProduse
                    : null
            );
        }

        public Task CreateCartAsync(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);

            return Task.Run(() =>
            {
                _memoryCache.GetOrCreate(userId, entry => new List<ProdusInCos>());
            });
        } //Functie de creare cos (se apeleaza la login)
        public async Task AddToCartAsync(string? userId, int idProdus, int cantitate)
        {
            ArgumentNullException.ThrowIfNull(userId);
            if (cantitate <= 0)
            {
                throw new Exception("Cantitatea trebuie sa fie mai mare ca 0!");
            }

            var produs = await _context.Produse.FindAsync(idProdus);
            if (produs == null || produs.Stoc < cantitate)
            {
                throw new InvalidOperationException("Stoc insuficient sau produs invalid.");
            }

            // calcul pret cu discount
            decimal pretPerUnitateCuReducere = produs.Pret - (produs.Pret * (produs.Reducere / 100m));
            pretPerUnitateCuReducere = pretPerUnitateCuReducere.TruncateTo(2);

            // Preia din memorie cosul sau daca a fost sters, creeaza unul nou
            var cosUtilizator = await GetCartFromMemoryAsync(userId);
            if (cosUtilizator == null)
            {
                cosUtilizator = new List<ProdusInCos>();
                _memoryCache.Set(userId, cosUtilizator);
            }

            var produsExistent = cosUtilizator.FirstOrDefault(c => c.IdProdusCos == idProdus);

            if (produsExistent == null)
            {
                cosUtilizator.Add(new ProdusInCos
                {
                    IdProdusCos = idProdus,
                    CantitateInCos = cantitate,
                    PretPerUnitate = pretPerUnitateCuReducere
                });
            }
            else
            {
                if (produs.Stoc < produsExistent.CantitateInCos + cantitate)
                {
                    throw new InvalidOperationException("Stoc insuficient pentru noua cantitate.");
                }
                produsExistent.CantitateInCos += cantitate;
            }

            _memoryCache.Set(userId, cosUtilizator);
        }
        //Functie pentru adaugarea in cosul de cumparaturi al unui produs

        public async Task UpdateCartItemAsync(string userId, int idProdus, int cantitateNoua)
        {
            var produs = await _context.Produse.FindAsync(idProdus);
            if (produs == null || produs.Stoc < cantitateNoua)
            {
                throw new InvalidOperationException("Stoc insuficient sau produs invalid.");
            }

            var cosUtilizator = await GetCartFromMemoryAsync(userId);
            if (cosUtilizator == null)
            {
                throw new InvalidOperationException("Cosul nu exista");
            }

            var item = cosUtilizator.FirstOrDefault(c => c.IdProdusCos == idProdus);
            if (item == null)
            {
                throw new InvalidOperationException("Produsul nu exista in cos.");
            }

            item.CantitateInCos = cantitateNoua;
            _memoryCache.Set(userId, cosUtilizator);
        } //Functie pentru actualizarea cantitatii din cos

        public async Task RemoveFromCartAsync(string userId, int productId)
        {
            var cosUtilizator = await GetCartFromMemoryAsync(userId);
            if (cosUtilizator == null) return;
            cosUtilizator.RemoveAll(c => c.IdProdusCos == productId);
            _memoryCache.Set(userId, cosUtilizator);
        } //Scoate un produs din cosul utilizatorului

        public async Task<int> PlaceOrderAsync(string userId)
        {
            var cosUtilizator = await GetCartFromMemoryAsync(userId);
            if (cosUtilizator == null || cosUtilizator.Count == 0)
            {
                throw new InvalidOperationException("Cosul este gol.");
            }

            // 1. Verifica stocul pentru toate produsele din cos si scoate din stoc
            foreach (var item in cosUtilizator)
            {
                var produs = await _context.Produse.FindAsync(item.IdProdusCos);
                if (produs == null || produs.Stoc < item.CantitateInCos)
                {
                    throw new InvalidOperationException($"Stoc insuficient pentru produsul cu ID {item.IdProdusCos}.");
                }
                produs.Stoc -= item.CantitateInCos;
            }

            // Salvam datele ca sa ne asiguram ca un alt user nu poate cumpara mai mult decat mai avem ramas
            await _context.SaveChangesAsync();

            // 2. Creare comanda
            decimal total = 0m;

            foreach (var item in cosUtilizator)
            {
                var produs = await _context.Produse.FindAsync(item.IdProdusCos);
                if (produs != null)
                {
                    total += item.PretPerUnitate * item.CantitateInCos;
                }
            }

            var comandaNoua = new Comanda
            {
                Proiect_ipUserID = userId,
                DataComanda = DateTime.UtcNow,
                PretTotal = total,
                PuncteGenerate = 0, // se pune 0, punctele se vor adauga dupa confirmarea comenzii
                PuncteUtilizate = 0,
                Reducere = 0.0m,
                Destinatar = "",
                Telefon = "",
                Adresa = "",
                CStatus = Comanda.ComandaStatus.InProcesare,
                PStatus = Comanda.PlataStatus.InProcesare
            };

            _context.Comenzi.Add(comandaNoua);
            await _context.SaveChangesAsync();

            // 3. Adaugare asociere produse cumparate cu comanda noua in tabelul de legatura
            foreach (var item in cosUtilizator)
            {
                var produs = await _context.Produse.FindAsync(item.IdProdusCos);
                if (produs != null)
                {
                    // pretul deja include discount-ul
                    _context.ComandaProduse.Add(new ComandaProdus
                    {
                        IdComanda = comandaNoua.IdComanda,
                        IdProdus = item.IdProdusCos,
                        Cantitate = item.CantitateInCos,
                        PretUnitar = item.PretPerUnitate
                    });
                }
            }

            await _context.SaveChangesAsync();

            // 4. Acorda puncte aferente comenzii - nu se vor mai acorda punctele la crearea comenzii!
            // string Motiv = $"Comanda {comandaNoua.IdComanda}";
            // await _pointsService.ModifyPointsAsync(userId, comandaNoua.PuncteGenerate, Motiv, comandaNoua.IdComanda);

            // 4. Goleste cosul
            _memoryCache.Remove(userId);

            return comandaNoua.IdComanda;
        } //Functia de trimitere a comenzii

        public async Task<List<(int ProductId, int Cantitate)>> GetProductsAsync(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            var cosUtilizator = await GetCartFromMemoryAsync(userId);
            List<(int ProductId, int Cantitate)> produse = new List<(int, int)>();
            if (cosUtilizator != null)
            {
                foreach (var item in cosUtilizator)
                {
                    produse.Add((item.IdProdusCos, item.CantitateInCos));
                }
            }
            return produse;
        } //Returneaza id-ul si cantitatea din fiecare produs din cos

        public async Task<(decimal Suma, int NrProduse)> GetCartDataAsync(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            var cosUtilizator = await GetCartFromMemoryAsync(userId);

            if (cosUtilizator == null || cosUtilizator.Count == 0)
            {
                return (0, 0);
            }

            decimal total = 0m;
            int nrProduse = 0;

            foreach (var item in cosUtilizator)
            {
                var produs = await _context.Produse.FindAsync(item.IdProdusCos);
                if (produs != null)
                {
                    total += item.PretPerUnitate * item.CantitateInCos;
                    nrProduse += item.CantitateInCos;
                }
            }

            return (total, nrProduse);
        }
    }
}
