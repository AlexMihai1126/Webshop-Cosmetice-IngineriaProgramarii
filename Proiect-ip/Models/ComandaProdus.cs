using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Proiect_ip.Areas.Identity.Data;

namespace Proiect_ip.Models
{
    public class ComandaProdus
    {
        public int Id { get; set; }
        public int IdComanda { get; set; }
        public Comanda Comanda { get; set; }
        public int IdProdus { get; set; }
        public Produs Produs { get; set; }
        public int Cantitate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PretUnitar { get; set; }
    }
}
