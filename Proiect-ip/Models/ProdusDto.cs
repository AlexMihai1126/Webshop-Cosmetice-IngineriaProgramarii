using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_ip.Models
{
    public class ProdusDto
    {
        [Required(ErrorMessage = "Nume is required")]
        public string Nume { get; set; }
        [Required(ErrorMessage = "Descriere is required")]
        public string Descriere { get; set; }
        [Required(ErrorMessage = "Pret is required")]
        public decimal Pret { get; set; }
        [Required(ErrorMessage = "Stoc is required")]
        public int Stoc { get; set; }
        public int CategorieId { get; set; }
    }
}