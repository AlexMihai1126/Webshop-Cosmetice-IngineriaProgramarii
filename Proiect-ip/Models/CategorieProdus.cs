namespace Proiect_ip.Models
{
    public class CategorieProdus
    {
        public int Id { get; set; }
        public string NumeCateg { get; set; }
        public string? Decscriere { get; set; }
        public ICollection<Produs> Produse { get; set; }
    }
}
