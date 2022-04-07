using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Midas.Data
{
    public class MidasContext : IdentityDbContext
    {
        public MidasContext(DbContextOptions<MidasContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produto { get; set; }
        public DbSet<Promocao> Promocao { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<Imagem> Imagem { get; set; }
        public DbSet<ApplicationUser> Usuario { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Compra> Compra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Produto>().Property(p => p.Valor).HasPrecision(15, 2);
            modelBuilder.Entity<Promocao>().Property(p => p.Porcentagem).HasPrecision(5, 2);

        }

    }
}
