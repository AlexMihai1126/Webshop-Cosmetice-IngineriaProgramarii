using System.ComponentModel.DataAnnotations;

namespace Proiect_ip.Models
{
    public class CategorieDto
    {
        [Required(ErrorMessage = "Nume is required")]
        public string Nume { get; set; }

        [Required(ErrorMessage = "Descriere is required")]
        public string Descriere { get; set; }
    }
}
