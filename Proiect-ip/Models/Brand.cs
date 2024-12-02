using System.ComponentModel.DataAnnotations;

namespace Proiect_ip.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Nume is required")]
        public string NumeBrand { get; set; }
        public IEnumerable<Produs> ProduseBrand { get; set; }
    }
}
