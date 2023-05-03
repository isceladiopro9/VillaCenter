using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using VillaCenter_Api.Models;

namespace VillaCenter_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options) { 
        
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                    new Villa() { 
                        Id=1,
                        Nombre="Villa Real inicial",
                        Detalle="Detalle de la Villa",
                        ImagenUrl="",
                        Ocupantes= 5,
                        MetrosCuadrados = 10,
                        Tarifa = 100,
                        Amenidad = "",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    },
                    new Villa()
                    {
                        Id = 2,
                        Nombre = "Premium Villa",
                        Detalle = "Detalle de la Villa",
                        ImagenUrl = "",
                        Ocupantes = 12,
                        MetrosCuadrados = 150,
                        Tarifa = 200,
                        Amenidad = "",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    }
                );
        }

    }
}
