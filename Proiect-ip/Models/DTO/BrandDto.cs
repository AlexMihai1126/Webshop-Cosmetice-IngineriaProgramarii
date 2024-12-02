using System.ComponentModel.DataAnnotations;

namespace Proiect_ip.Models.DTO
{
    public class BrandDto
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Nume { get; set; }
    }
}
