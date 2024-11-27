using Proiect_ip.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
namespace Proiect_ip.Models
{
    public class Comanda
    {
        public int IdComanda { get; set; }
        public string Proiect_ipUserID { get; set; }
        public Proiect_ipUser Utilizator { get; set; }
        public ICollection<ComandaProdus> ComandaProduse { get; set; }
        public ICollection<Produs> Produse { get; set; }
        public DateTime DataComanda { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PretTotal { get; set; }
        public int PuncteGenerate { get; set; }
    }

}
