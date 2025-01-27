using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Data;
using Proiect_ip.Models;
using Proiect_ip.Services.DataCache;
using Proiect_ip.Services.Config;
using System.ComponentModel.DataAnnotations;
using Proiect_ip.Services;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Proiect_ip.Pages
{
    [Authorize]
    public class PlataModel : PageModel
    {
        private readonly Proiect_ipContext _context;
        private readonly UserManager<Proiect_ipUser> _userManager;
        private readonly PointsService _pointsService;
        private readonly ShoppingCartService _cartService;

        public PlataModel(Proiect_ipContext context, UserManager<Proiect_ipUser> userManager, PointsService pointsService, ShoppingCartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _pointsService = pointsService;
            _cartService = cartService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Destinatarul este obligatoriu.")]
        public string Destinatar { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Numărul de telefon este obligatoriu.")]
        [Phone(ErrorMessage = "Numărul de telefon nu este valid.")]
        public string Telefon { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Adresa este obligatorie.")]
        public string Adresa { get; set; }

        [BindProperty]
        public bool UsePoints { get; set; }
        public int PointsBalance { get; set; }
        public int PointsUsed { get; set; }
        public decimal ReducereComanda { get; set; }
        public decimal InitialPrice { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var (totalCos, nrProduse) = await _cartService.GetCartDataAsync(userId);

                if (totalCos == 0 && nrProduse == 0)
                {
                    TempData["ErrorMessage"] = "Nu puteți plăti o comandă goală.";
                    return RedirectToPage("/Cos");
                }

                PointsBalance = await _pointsService.GetPointsAsync(userId);

                if (PointsBalance > 0)
                {
                    var maxDiscount = totalCos * OrderDiscounts.DiscountMaximPerComanda;

                    // calcul numar de puncte ce pot fi folosite
                    PointsUsed = (int)Math.Min(PointsBalance, maxDiscount / OrderDiscounts.RataConversiePuncte);

                    // calcul reducere posibila
                    ReducereComanda = Math.Round(PointsUsed * OrderDiscounts.RataConversiePuncte, 2);
                }
                else
                {
                    PointsUsed = 0;
                    ReducereComanda = 0;
                }

                InitialPrice = totalCos;

                return Page();
            }
            catch(Exception)
            {
                TempData["ErrorMessage"] = "Eroare la preluarea datelor. Va rugam sa incercati din nou.";
                return RedirectToPage("/Cos");
            }

        }

        public async Task<IActionResult> OnPostAsync(string PaymentMethod)
        {
            if (!ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var (totalCos, nrProduse) = await _cartService.GetCartDataAsync(userId);

                if (totalCos == 0 && nrProduse == 0)
                {
                    TempData["ErrorMessage"] = "Nu puteți plăti o comandă goală.";
                    return RedirectToPage("/Cos");
                }

                PointsBalance = await _pointsService.GetPointsAsync(userId);

                if (PointsBalance > 0)
                {
                    var maxDiscount = totalCos * OrderDiscounts.DiscountMaximPerComanda;

                    PointsUsed = (int)Math.Min(PointsBalance, maxDiscount / OrderDiscounts.RataConversiePuncte);
                    ReducereComanda = Math.Round(PointsUsed * OrderDiscounts.RataConversiePuncte, 2);
                }
                else
                {
                    PointsUsed = 0;
                    ReducereComanda = 0;
                }

                InitialPrice = totalCos;
                return Page();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userId = _userManager.GetUserId(User);
                    var (totalCos, nrProduse) = await _cartService.GetCartDataAsync(userId);
                    var pointsBalanceCheck = await _pointsService.GetPointsAsync(userId);

                    var cosUtilizator = await _cartService.GetCartFromMemoryAsync(userId);

                    // pre-check stoc
                    foreach (var itemCheck in cosUtilizator)
                    {
                        var produs = await _context.Produse.FindAsync(itemCheck.IdProdusCos);
                        if (produs != null)
                        {
                            if (produs.Stoc < itemCheck.CantitateInCos)
                            {
                                TempData["ErrorMessage"] = $"Produsul '{produs.Nume}' nu este disponibil în cantitatea solicitată ({itemCheck.CantitateInCos}). Stoc disponibil: {produs.Stoc}.";
                                return RedirectToPage("/Cos");
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = $"Produsul cu ID {itemCheck.IdProdusCos} nu a fost găsit.";
                            return RedirectToPage("/Cos");
                        }
                    }

                    decimal FinalPrice;

                    if (pointsBalanceCheck > 0 && UsePoints == true)
                    {
                        var maxDiscount = totalCos * OrderDiscounts.DiscountMaximPerComanda;

                        PointsUsed = (int)Math.Min(pointsBalanceCheck, maxDiscount / OrderDiscounts.RataConversiePuncte);
                        ReducereComanda = Math.Round(PointsUsed * OrderDiscounts.RataConversiePuncte, 2);
                        FinalPrice = Math.Round(totalCos - ReducereComanda, 2);
                    }
                    else
                    {
                        PointsUsed = 0;
                        ReducereComanda = 0;
                        FinalPrice = totalCos;
                    }

                    var comandaNoua = new Comanda
                    {
                        Proiect_ipUserID = userId,
                        DataComanda = DateTime.UtcNow,
                        PretTotal = FinalPrice,
                        PuncteGenerate = (int)Math.Ceiling(FinalPrice),
                        PuncteUtilizate = PointsUsed,
                        Reducere = ReducereComanda,
                        Destinatar = Destinatar,
                        Telefon = Telefon,
                        Adresa = Adresa,
                        CStatus = Comanda.ComandaStatus.InProcesare,
                        PStatus = Comanda.PlataStatus.InProcesare
                    };

                    _context.Comenzi.Add(comandaNoua);
                    await _context.SaveChangesAsync();

                    foreach (var item in cosUtilizator)
                    {
                        var produs = await _context.Produse.FindAsync(item.IdProdusCos);
                        if (produs != null)
                        {
                            if (produs.Stoc >= item.CantitateInCos)
                            {
                                _context.ComandaProduse.Add(new ComandaProdus
                                {
                                    IdComanda = comandaNoua.IdComanda,
                                    IdProdus = item.IdProdusCos,
                                    Cantitate = item.CantitateInCos,
                                    PretUnitar = item.PretPerUnitate
                                });

                                produs.Stoc -= item.CantitateInCos;
                                _context.Produse.Update(produs);
                            }
                            else
                            {
                                throw new InvalidOperationException($"Insufficient stock for product ID {item.IdProdusCos}");
                            }
                        }
                    }

                    if (PaymentMethod == "Numerar")
                    {
                        comandaNoua.PStatus = Comanda.PlataStatus.Numerar;
                    }
                    else if (PaymentMethod == "Card")
                    {
                        comandaNoua.PStatus = Comanda.PlataStatus.Acceptata;
                    }

                    await _context.SaveChangesAsync();

                    _cartService.RemoveUserCart(userId);

                    if (UsePoints == true)
                    {
                        await _pointsService.ModifyPointsAsync(userId, -PointsUsed, $"Reducere la comanda cu ID {comandaNoua.IdComanda}", comandaNoua.IdComanda);
                    }

                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = $"Comanda cu ID-ul {comandaNoua.IdComanda} a fost plasata cu succes.";
                    if (PaymentMethod == "Card")
                    {
                        TempData["SuccessMessage"] += " Plata a fost procesată cu succes.";
                    }

                    return RedirectToPage("/Account/Manage/MyOrders", new { area = "Identity" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    TempData["ErrorMessage"] = "Eroare la finalizarea comenzii. Va rugam sa incercati din nou." + ex.ToString();
                    return RedirectToPage("/Cos");
                }
            }

        }


    }
}
