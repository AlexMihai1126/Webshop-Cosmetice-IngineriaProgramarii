using Proiect_ip.Areas.Identity.Data;
using System;

namespace Proiect_ip.Models
{
    public class IstoricPuncte
    {
        public int Id { get; set; }
        public string Proiect_ipUserID { get; set; }
        public int Puncte { get; set; }
        public string? Motiv { get; set; }
        public DateTime DataAdaugare { get; set; }
        public int? IdComanda { get; set; }

        //Navigation properties
        public Comanda Comanda { get; set; }
        public Proiect_ipUser User { get; set; }
    }
}
