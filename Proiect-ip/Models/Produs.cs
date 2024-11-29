using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_ip.Models
{
    public class Produs
    {
        public int IdProdus { get; set; }
        public int? IdCategorie { get; set; }
        public int? IdVoucher { get; set; }
        public string Nume { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Pret { get; set; }
        public string Descriere { get; set; }
        public int Stoc { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }
        
        //Navigation properties
        public CategorieProdus Categorie { get; set; }
        public Voucher Voucher { get; set; }
        public ICollection<Comanda> Comenzi { get; set; }
        public ICollection<ComandaProdus> ComandaProduse { get; set; }
    }

}
