using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
namespace Proiect_ip.Services;
public class OrdersManagerService(Proiect_ipContext context)
{
    public async Task<List<Comanda>> GetAllOrdersAsync(Comanda.ComandaStatus? statusFilter, bool isLatest)
    {
        IQueryable<Comanda> query = context.Comenzi.Include(c => c.Produse).Include(c => c.Utilizator).Include(c => c.ComandaProduse).ThenInclude(cp => cp.Produs);
        if (isLatest)
        {
            query = query.OrderByDescending(c => c.DataComanda);
            //Ordonare dupa cele mai recente sau cele mai vechi
        }

        if(statusFilter != Comanda.ComandaStatus.Toate)
        {
            query = query.Where(c => c.Status == statusFilter);
            //Filtrare dupa status
        }

        return await query.ToListAsync();
    }//Adminul poate vedea toate comenzile din site si poate vedea rezultatele filtrate

    public async Task UpdateOrderStatusAsync(int orderId, Comanda.ComandaStatus newStatus)
    {
        var order = await context.Comenzi.FindAsync(orderId);
        if (order == null)
            throw new InvalidOperationException("Order not found");

        order.Status = newStatus;
        context.Comenzi.Update(order);
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

    public async Task CancelOrderAsync(int orderId, string userId)
    {
        var order = await context.Comenzi.FindAsync(orderId);
        if (order == null || order.Proiect_ipUserID != userId)
            throw new InvalidOperationException("Order not found or not authorized");

        if (order.Status != Comanda.ComandaStatus.InProcesare)
            throw new InvalidOperationException("Cannot cancel this order");

        order.Status = Comanda.ComandaStatus.Anulat;
        context.Comenzi.Update(order);
        await context.SaveChangesAsync();
    }//Clientul isi poate anula singur comanda
}
