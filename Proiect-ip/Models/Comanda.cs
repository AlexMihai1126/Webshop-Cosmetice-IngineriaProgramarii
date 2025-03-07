﻿using Proiect_ip.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Proiect_ip.Models
{
    public class Comanda
    {
        public enum ComandaStatus
        {
            Toate = 0,
            Anulat = 1,
            InProcesare = 2,
            Confirmata = 3,
            Expediata = 4
        }

        public enum PlataStatus
        {
            Toate = 0,
            InProcesare = 1,
            Acceptata = 2,
            Anulata = 3,
            Numerar = 4
        }
        public int IdComanda { get; set; }
        public string Proiect_ipUserID { get; set; }
        public DateTime DataComanda { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PretTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Reducere { get; set; }
        public int PuncteGenerate { get; set; }
        public int PuncteUtilizate { get; set; }
        [Required]
        public string Destinatar { get; set; }
        [Required]
        public string Telefon { get; set; }
        [Required]
        public string Adresa { get; set; }

        //Navigation properties
        public ComandaStatus CStatus { get; set; }
        public PlataStatus PStatus { get; set; }
        public Proiect_ipUser Utilizator { get; set; }
        public ICollection<ComandaProdus> ComandaProduse { get; set; }
        public ICollection<Produs> Produse { get; set; }
    }

}
