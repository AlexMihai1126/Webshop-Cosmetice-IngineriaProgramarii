using System.ComponentModel.DataAnnotations;

namespace Proiect_ip.Models
{
    public class CategorieProdus
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nume is required")]
        public string NumeCateg { get; set; }
        public string? Descriere { get; set; }
        public ICollection<Produs> ProduseCategorie { get; set; }
    }
}
