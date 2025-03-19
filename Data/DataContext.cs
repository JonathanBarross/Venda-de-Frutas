using Microsoft.EntityFrameworkCore;
using Venda_de_Frutas.Models;

namespace Venda_de_Frutas.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Fruta> Frutas { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Administrador",
                    Senha = "senha123", 
                    Perfil = "Administrador"
                },
                new Usuario
                {
                    Id = 2,
                    Nome = "Vendedor",
                    Senha = "senha456", 
                    Perfil = "Vendedor"
                }
            );
        }
        
    }
}