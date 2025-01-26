using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Models;
using System.ComponentModel.DataAnnotations;

namespace Proiect_ip.Pages
{
    public class PlataModel : PageModel
    {
        private readonly Proiect_ipContext _context;

        public PlataModel(Proiect_ipContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PaymentInputModel Input { get; set; }

        public class PaymentInputModel
        {
            public int IdComanda { get; set; }

            [Required(ErrorMessage = "Destinatarul este obligatoriu.")]
            public string Destinatar { get; set; }

            [Required(ErrorMessage = "Numărul de telefon este obligatoriu.")]
            [Phone(ErrorMessage = "Numărul de telefon nu este valid.")]
            public string Telefon { get; set; }

            [Required(ErrorMessage = "Adresa este obligatorie.")]
            public string Adresa { get; set; }

            public Comanda.PlataStatus PlataStatus { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var order = await _context.Comenzi.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            Input = new PaymentInputModel
            {
                IdComanda = order.IdComanda,
                Destinatar = order.Destinatar,
                Telefon = order.Telefon,
                Adresa = order.Adresa,
                PlataStatus = order.PStatus
            };

            return Page();
        } //TODO - de validat user-ul ca ii apartine comanda, ca nu a fost confirmata anterior si ca status plata este confirmat

        public async Task<IActionResult> OnPostAsync(string PaymentMethod)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var comanda = await _context.Comenzi.FindAsync(Input.IdComanda);
            if (comanda == null)
            {
                return NotFound();
            }

            if (PaymentMethod == "Numerar")
            {
                comanda.PStatus = Comanda.PlataStatus.Numerar;
            }
            else if (PaymentMethod == "Card")
            {
                comanda.PStatus = Comanda.PlataStatus.Acceptata;
            }

            comanda.Destinatar = Input.Destinatar;
            comanda.Telefon = Input.Telefon;
            comanda.Adresa = Input.Adresa;

            await _context.SaveChangesAsync();

            string successMessage = $"Comanda cu ID-ul {Input.IdComanda} a fost plasata cu succes.";
            if (PaymentMethod == "Card")
            {
                successMessage += " Plata a fost procesată cu succes.";
            }

            TempData["SuccessMessage"] = successMessage;
            return RedirectToPage("/Account/Manage/MyOrders", new { area = "Identity" });
        }

    }
}
