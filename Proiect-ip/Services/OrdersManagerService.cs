using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services.DataCache;
namespace Proiect_ip.Services;
public class OrdersManagerService(Proiect_ipContext context, PointsService pointsService)
{
    private const float RataConversiePuncte = 0.1f; // 1p = 0.1 LEI
    private const float DiscountMaximPerComanda = 0.25f; //25% din valoarea totala a comenzii poate fi redusa
    public async Task<List<Comanda>> GetAllOrdersAsync(Comanda.ComandaStatus? statusFilter, bool sortareDupaRecente)
    {
        IQueryable<Comanda> query = context.Comenzi.Include(c => c.Produse).Include(c => c.Utilizator).Include(c => c.ComandaProduse).ThenInclude(cp => cp.Produs);
        if (sortareDupaRecente)
        {
            query = query.OrderByDescending(c => c.DataComanda);
            //Ordonare dupa cele mai recente sau cele mai vechi
        }

        if(statusFilter != Comanda.ComandaStatus.Toate)
        {
            query = query.Where(c => c.CStatus == statusFilter);
            //Filtrare dupa status
        }

        return await query.ToListAsync();
    }//Adminul poate vedea toate comenzile din site si poate vedea rezultatele filtrate

    public async Task UpdateOrderStatusAsync(int idComanda, Comanda.ComandaStatus newStatus)
    {
        var comanda = await context.Comenzi.FindAsync(idComanda);
        if (comanda == null)
            throw new InvalidOperationException("Order not found");

        comanda.CStatus = newStatus;
        context.Comenzi.Update(comanda);
        await context.SaveChangesAsync();
    }//Adminul poate schimba statusul unei comenzi

    public async Task<List<Comanda>> GetCustomerOrdersAsync(string userId)
    {
        return await context.Comenzi
            .Where(o => o.Proiect_ipUserID == userId)
            .Include(o => o.Produse)
            .Include(o => o.ComandaProduse)
                .ThenInclude(cp => cp.Produs)
            .ToListAsync();
    } //Clientul isi poate vedea propriile comenzi

    public async Task CancelOrderAsync(int idComanda, string userId)
    {
        var comanda = await context.Comenzi.FindAsync(idComanda);
        if (comanda == null || comanda.Proiect_ipUserID != userId)
            throw new InvalidOperationException("Order not found or not authorized");

        if (comanda.CStatus != Comanda.ComandaStatus.InProcesare)
            throw new InvalidOperationException("Cannot cancel this order");

        comanda.CStatus = Comanda.ComandaStatus.Anulat;
        context.Comenzi.Update(comanda);
        await context.SaveChangesAsync();
    }//Clientul isi poate anula singur comanda

    public async Task AddDiscountAsync(int idComanda, string userId)
    {
        var comanda = await context.Comenzi.FindAsync(idComanda);
        if (comanda == null || comanda.Proiect_ipUserID != userId)
            throw new InvalidOperationException("Comanda negasita sau utilizatorul nu are acces.");

        if (comanda.CStatus != Comanda.ComandaStatus.InProcesare)
            throw new InvalidOperationException("Nu se mai pot adauga reduceri dupa faza de procesare.");

        var puncteUtilizator = await pointsService.GetPointsAsync(userId);

        if (puncteUtilizator <= 0)
            throw new InvalidOperationException("Utilizatorul nu are puncte disponibile.");

        var valoareDiscountMaxim = comanda.PretTotal * (decimal)DiscountMaximPerComanda;
        var valoarePuncte = (decimal)puncteUtilizator * (decimal)RataConversiePuncte;
        var discount = Math.Min(valoareDiscountMaxim, valoarePuncte);
        comanda.PretTotal -= discount;

        var puncteUtilizate = (int)(discount / (decimal)RataConversiePuncte);
        string? motiv = $"Reducere aplicata la comanda {comanda.IdComanda}";
        try
        {
            await pointsService.ModifyPointsAsync(userId, -puncteUtilizate, motiv, comanda.IdComanda);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException($"Eroare la scaderea punctelor: {ex.Message}");
        }

        context.Comenzi.Update(comanda);
        await context.SaveChangesAsync();
    } //TODO - utilizatorul va trebui sa primeasca puncte aferente comenzii dupa ce se aplica reducerea (daca face comanda de 100, aplica 25% reducere tre sa mai primeasca doar 75 puncte);

}
