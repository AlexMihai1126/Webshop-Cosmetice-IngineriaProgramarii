using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services.DataCache;
namespace Proiect_ip.Services;
public class OrdersManagerService(Proiect_ipContext context, PointsService pointsService)
{
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
        var comanda = await context.Comenzi
        .Include(c => c.ComandaProduse)
        .FirstOrDefaultAsync(c => c.IdComanda == idComanda);
        if (comanda == null)
            throw new InvalidOperationException("Order not found");

        if (newStatus == Comanda.ComandaStatus.Anulat)
        {
            comanda.PStatus = Comanda.PlataStatus.Anulata;

            // restaurare produse in stoc pentru comanda anulata
            foreach (var orderItem in comanda.ComandaProduse)
            {
                var produs = await context.Produse.FindAsync(orderItem.IdProdus);
                if (produs != null)
                {
                    produs.Stoc += orderItem.Cantitate;
                    context.Produse.Update(produs);
                }
            }

            context.ComandaProduse.RemoveRange(comanda.ComandaProduse);

            if (comanda.PuncteUtilizate > 0)
            {
                await pointsService.ModifyPointsAsync(comanda.Proiect_ipUserID, +comanda.PuncteUtilizate, $"Anulare comanda {idComanda}.", idComanda);
            } // restaureaza punctele consumate utilizatorului
        }

        if (newStatus == Comanda.ComandaStatus.Confirmata)
        {
            // cand se confirma nu se intampla nimic extra
        }

        if(newStatus == Comanda.ComandaStatus.Expediata)
        {
            await pointsService.ModifyPointsAsync(comanda.Proiect_ipUserID, comanda.PuncteGenerate, $"Adaugare puncte pentru comanda {idComanda}.", idComanda);
        }

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
            .OrderByDescending(o => o.DataComanda)
            .ToListAsync();
    } //Clientul isi poate vedea propriile comenzi

    public async Task CancelOrderAsync(int idComanda, string userId)
    {
        var comanda = await context.Comenzi
        .Include(c => c.ComandaProduse)
        .FirstOrDefaultAsync(c => c.IdComanda == idComanda);
        if (comanda == null || comanda.Proiect_ipUserID != userId)
            throw new InvalidOperationException("Comandă negăsită sau nu aveți acces.");

        if (comanda.CStatus != Comanda.ComandaStatus.InProcesare)
            throw new InvalidOperationException("Comanda nu poate fi anulată.");

        comanda.CStatus = Comanda.ComandaStatus.Anulat;
        comanda.PStatus = Comanda.PlataStatus.Anulata;
        context.Comenzi.Update(comanda);

        // restaurare produse in stoc pentru comanda anulata
        foreach (var orderItem in comanda.ComandaProduse)
        {
            var produs = await context.Produse.FindAsync(orderItem.IdProdus);
            if (produs != null)
            {
                produs.Stoc += orderItem.Cantitate;
                context.Produse.Update(produs);
            }
        }

        context.ComandaProduse.RemoveRange(comanda.ComandaProduse);
        await context.SaveChangesAsync();

        if (comanda.PuncteUtilizate > 0)
        {
            await pointsService.ModifyPointsAsync(comanda.Proiect_ipUserID, +comanda.PuncteUtilizate, $"Anulare comanda {idComanda}.", idComanda);
        } // restaureaza punctele consumate utilizatorului

    }//Clientul isi poate anula singur comanda

}
