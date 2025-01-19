using Proiect_ip.Areas.Identity.Data;

namespace Proiect_ip.Models
{
    public class UserMetrics
    {
        public int Id { get; set; }
        public string Proiect_ipUserID { get; set; }
        public decimal CheltuieliTotale { get; set; } 
        public int Nivel { get; set; }
        public DateTime UltimaActualizareNivel { get; set; }

        // Navigation property
        public Proiect_ipUser Utilizator { get; set; }
    }
}
