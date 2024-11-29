using Proiect_ip.Areas.Identity.Data;
using System;

namespace Proiect_ip.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Cod { get; set; }
        public DateTime DataExpirare { get; set; }
        public string Proiect_ipUserID { get; set; }

        //Navigation properties
        public Proiect_ipUser CreatDe { get; set; }
        public ICollection<Produs> Produse { get; set; }
    }
}
