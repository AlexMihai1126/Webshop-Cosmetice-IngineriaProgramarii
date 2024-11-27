using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Models;
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
        public DbSet<Voucher> Vouchere { get; set; }
        public DbSet<CategorieProdus> CategoriiProduse { get; set; }
        public DbSet<IstoricPuncte> IstoricPuncte { get; set; }
        public DbSet<ComandaProdus> ComandaProduse { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relatie dintre user si comanda
            builder.Entity<Comanda>()
                .HasKey(c => c.IdComanda);

            builder.Entity<Comanda>()
                .Property(c => c.Status)
                .HasConversion(
                    v => v.ToString(), // Enum in string
                    v => (ComandaStatus)Enum.Parse(typeof(ComandaStatus), v)); // Conversie din string inapoi in Enum la citire

            builder.Entity<Comanda>()
                .HasOne(c => c.Utilizator)
                .WithMany(u => u.Comenzi)
                .HasForeignKey(c => c.Proiect_ipUserID)
                .OnDelete(DeleteBehavior.NoAction); // Daca se sterge un user nu i se sterg si comenzile

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

            // Relatie intre produs si categorie produs
            builder.Entity<Produs>()
                .HasKey(p => p.IdProdus);

            builder.Entity<Produs>()
                .HasOne(p => p.Categorie)
                .WithMany(c => c.Produse)
                .HasForeignKey(p => p.IdCategorie);

            // Relatie intre produs si voucher
            builder.Entity<Produs>()
                .HasOne(p => p.Voucher)
                .WithMany(v => v.Produse)
                .HasForeignKey(p => p.IdVoucher)
                .OnDelete(DeleteBehavior.SetNull);

            // Relatie M:M intre Comanda si Produs descompusa in tabelul ComandaProdus
            builder.Entity<Produs>()
                .HasMany(p => p.Comenzi)
                .WithMany(c => c.Produse)
                .UsingEntity<ComandaProdus>(j =>
                {
                    j.HasKey(cp => new { cp.IdComanda, cp.IdProdus }); //PK compus
                    j.Property(cp => cp.Cantitate).IsRequired();
                });

            // Voucher
            builder.Entity<Voucher>()
                .HasKey(v => v.Id); // PK

            builder.Entity<Voucher>()
                .HasOne(v => v.CreatDe) // 1:M cu tabelul User
                .WithMany(u => u.Vouchere)
                .HasForeignKey(v => v.Proiect_ipUserID)
                .OnDelete(DeleteBehavior.Cascade); //Se sterg voucherele create de acel user

            // CategorieProdus
            builder.Entity<CategorieProdus>()
                .HasKey(c => c.Id); // PK

            builder.Entity<CategorieProdus>()
                .HasMany(c => c.Produse) // Relatie 1:M cu tabelul Produs
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
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<IstoricPuncte>()
                .HasOne(i => i.Comanda) // Cheie straina pt entitatea Comanda
                .WithMany()
                .HasForeignKey(i => i.IdComanda)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
