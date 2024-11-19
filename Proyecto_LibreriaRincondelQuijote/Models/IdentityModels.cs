using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

using System.Collections.Generic;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
    // ApplicationUser: Extiende IdentityUser para agregar datos adicionales si es necesario
    public class ApplicationUser : IdentityUser
    {
       
       

        public ICollection<Carrito> Carritos { get; set; }  // Relación con Carrito
        public ICollection<HistorialCompra> HistorialCompras { get; set; }  // Relación con Historial_Compras
        public ICollection<Resena> Resenas { get; set; }  // Relación con Reseña

        // Este método genera un ClaimsIdentity que puede ser usado para la autenticación
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    // DbContext para Identity (y tu base de datos general)
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoProducto> CarritoProductos { get; set; }
        public DbSet<HistorialCompra> HistorialCompras { get; set; }
        public DbSet<Resena> Resenas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación entre Carrito y Usuario
            modelBuilder.Entity<Carrito>()
                .HasKey(c => c.CodigoCarrito);

            // Configuración de la relación entre HistorialCompra y Usuario
            modelBuilder.Entity<HistorialCompra>()
                .HasKey(h => h.CodigoHistorial);

            // Configuración de la relación entre Producto y Estado
            modelBuilder.Entity<Producto>()
                .HasKey(p => p.CodigoProducto);

            // Configuración de la relación entre Categoria y Producto
            modelBuilder.Entity<Categoria>()
                .HasKey(c => c.CodigoCategoria);

            // Configuración de la relación entre Estado y Producto
            modelBuilder.Entity<Estado>()
                .HasKey(e => e.CodigoEstado);

            // Configuración de la relación entre Resena y Producto
            modelBuilder.Entity<Resena>()
                .HasKey(r => r.CodigoResena);

            // Configuración de la relación muchos a muchos entre Carrito y Producto (Carrito_Producto)
            modelBuilder.Entity<CarritoProducto>()
                .HasKey(cp => new { cp.CodigoCarrito, cp.CodigoProducto });  // Definir la clave primaria compuesta

            modelBuilder.Entity<CarritoProducto>()
                .HasRequired(cp => cp.Carrito)
                .WithMany(c => c.CarritosProductos)
                .HasForeignKey(cp => cp.CodigoCarrito)
                .WillCascadeOnDelete(false); // Evitar el borrado en cascada del carrito cuando se elimina un producto

            modelBuilder.Entity<CarritoProducto>()
                .HasRequired(cp => cp.Producto)
                .WithMany(p => p.CarritosProductos)
                .HasForeignKey(cp => cp.CodigoProducto)
                .WillCascadeOnDelete(false); // Evitar el borrado en cascada del producto cuando se elimina un carrito
        }
    }
}
