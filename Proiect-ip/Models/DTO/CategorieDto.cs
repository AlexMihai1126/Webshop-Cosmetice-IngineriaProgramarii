using System.ComponentModel.DataAnnotations;

namespace Proiect_ip.Models.DTO
{
    public class CategorieDto
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Nume { get; set; }

        public string? Descriere { get; set; }
    }
}
