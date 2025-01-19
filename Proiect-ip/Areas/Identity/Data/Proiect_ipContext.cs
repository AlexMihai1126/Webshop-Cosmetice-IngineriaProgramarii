using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Models;
using System.Reflection.Emit;
using static Proiect_ip.Models.Comanda;

namespace Proiect_ip.Data
{
    public class Proiect_ipContext : IdentityDbContext<Proiect_ipUser>
    {
        public Proiect_ipContext(DbContextOptions<Proiect_ipContext> options)
            : base(options)
        {
        }

        public DbSet<Comanda> Comenzi { get; set; }
        public DbSet<Produs> Produse { get; set; }
        public DbSet<CategorieProdus> CategoriiProduse { get; set; }
        public DbSet<IstoricPuncte> IstoricPuncte { get; set; }
        public DbSet<ComandaProdus> ComandaProduse { get; set; }
        public DbSet<Brand> Branduri { get; set; }
        public DbSet<UserMetrics> UserMetrics { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relatie dintre user si comanda
            builder.Entity<Comanda>()
                .HasKey(c => c.IdComanda);

            builder.Entity<Comanda>()
                .Property(c => c.CStatus)
                .HasConversion(
                    v => v.ToString(), // Enum in string
                    v => (ComandaStatus)Enum.Parse(typeof(ComandaStatus), v)); // Conversie din string inapoi in Enum la citire

            builder.Entity<Comanda>()
                .Property(c => c.PStatus)
                .HasConversion(
                    v => v.ToString(), // Enum in string
                    v => (PlataStatus)Enum.Parse(typeof(PlataStatus), v)); // Conversie din string inapoi in Enum la citire

            builder.Entity<Comanda>()
                .HasOne(c => c.Utilizator)
                .WithMany(u => u.Comenzi)
                .HasForeignKey(c => c.Proiect_ipUserID)
                .OnDelete(DeleteBehavior.Cascade); // Daca se sterge un user i se sterg si comenzile

            // Relatie dintre Produs si ComandaProdus
            builder.Entity<ComandaProdus>()
                .HasKey(cp => new { cp.IdComanda, cp.IdProdus }); // PK compus

            builder.Entity<ComandaProdus>()
                .HasOne(cp => cp.Comanda)
                .WithMany(c => c.ComandaProduse)
                .HasForeignKey(cp => cp.IdComanda);

            builder.Entity<ComandaProdus>()
                .HasOne(cp => cp.Produs)
                .WithMany(p => p.ComandaProduse)
                .HasForeignKey(cp => cp.IdProdus);

            //Brand cu produs
            builder.Entity<Brand>()
                .HasKey(b => b.BrandId);

            builder.Entity<Brand>()
                .HasMany(b => b.ProduseBrand) // Relatie 1:M cu tabelul Produs
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.IdBrand)
                .OnDelete(DeleteBehavior.Cascade); //Se sterge produsul daca se sterge brandul

            // Produs
            builder.Entity<Produs>()
                .HasKey(p => p.IdProdus);

            // Relatie M:M intre Comanda si Produs descompusa in tabelul ComandaProdus
            builder.Entity<Produs>()
                .HasMany(p => p.Comenzi)
                .WithMany(c => c.Produse)
                .UsingEntity<ComandaProdus>(j =>
                {
                    j.HasKey(cp => new { cp.IdComanda, cp.IdProdus }); //PK compus
                    j.Property(cp => cp.Cantitate).IsRequired();
                });

            // CategorieProdus
            builder.Entity<CategorieProdus>()
                .HasKey(c => c.Id); // PK

            builder.Entity<CategorieProdus>()
                .HasMany(c => c.ProduseCategorie) // Relatie 1:M cu tabelul Produs
                .WithOne(p => p.Categorie)
                .HasForeignKey(p => p.IdCategorie)
                .OnDelete(DeleteBehavior.SetNull); //Se sterge categoria din produs daca se sterge categoria

            //IstoricPuncte
            builder.Entity<IstoricPuncte>()
                .HasKey(i => i.Id); // PK

            builder.Entity<IstoricPuncte>()
                .HasOne(i => i.User) //Relatie 1:M cu User
                .WithMany(u => u.IstoricPuncte)
                .HasForeignKey(i => i.Proiect_ipUserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IstoricPuncte>()
                .HasOne(i => i.Comanda) // Cheie straina pt entitatea Comanda
                .WithMany()
                .HasForeignKey(i => i.IdComanda);

            builder.Entity<UserMetrics>()
                .HasKey(um => um.Id); // PK

            builder.Entity<UserMetrics>()
                .HasOne(um => um.Utilizator)
                .WithOne(u => u.UserMetrics)
                .HasForeignKey<UserMetrics>(um => um.Proiect_ipUserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserMetrics>()
                .Property(um => um.CheltuieliTotale)
                .HasColumnType("decimal(10, 2)")
                .HasDefaultValue(0.0m)
                .IsRequired();

            builder.Entity<UserMetrics>()
                .Property(um => um.Nivel)
                .HasDefaultValue(1)
                .IsRequired();

            builder.Entity<UserMetrics>()
                .Property(um => um.UltimaActualizareNivel)
                .IsRequired();

        }
    }
}
