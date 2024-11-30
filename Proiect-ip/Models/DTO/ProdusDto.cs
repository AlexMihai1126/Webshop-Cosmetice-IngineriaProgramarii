using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Proiect_ip.Models.DTO
{
    public class ProdusDto
    {
        [Required(ErrorMessage = "Nume is required")]
        public string Nume { get; set; }
        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Descriere is required")]
        public string Descriere { get; set; }

        [Required(ErrorMessage = "Pret is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Pret must be greater than zero")]
        public decimal Pret { get; set; }

        [Required(ErrorMessage = "Stoc is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stoc must be a non-negative number")]
        public int Stoc { get; set; }

        public int? CategorieId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
