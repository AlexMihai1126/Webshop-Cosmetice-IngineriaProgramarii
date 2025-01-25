using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Proiect_ip.Models.DTO
{
    public class ProdusDto
    {
        [Required(ErrorMessage = "Nume este obligatoriu")]
        public string Nume { get; set; }
        [Required(ErrorMessage = "Brand este obligatoriu")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Descriere este obligatoriu")]
        public string Descriere { get; set; }

        [Required(ErrorMessage = "Pret este obligatoriu")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Pret must be greater than zero")]
        public decimal Pret { get; set; }

        [Required(ErrorMessage = "Stoc este obligatoriu")]
        [Range(0, int.MaxValue, ErrorMessage = "Stoc >= 0")]
        public int Stoc { get; set; }
        [Range(0, 90, ErrorMessage = "Reducerea trebuie sa fie intre 0 si 90.")]
        public int Reducere { get; set; }

        public int? CategorieId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
