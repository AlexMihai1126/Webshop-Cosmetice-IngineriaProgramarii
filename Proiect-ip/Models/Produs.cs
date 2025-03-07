﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_ip.Models
{
    public class Produs
    {
        public int IdProdus { get; set; }
        public int? IdCategorie { get; set; }
        public string Nume { get; set; }
        public int IdBrand { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Pret { get; set; }
        [DefaultValue(0)]
        [Range(0, 90, ErrorMessage = "Reducerea trebuie sa fie intre 0 si 90.")]
        public int Reducere { get; set; }
        public string Descriere { get; set; }
        public int Stoc { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }
        
        //Navigation properties
        public CategorieProdus Categorie { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Comanda> Comenzi { get; set; }
        public ICollection<ComandaProdus> ComandaProduse { get; set; }
    }

}
