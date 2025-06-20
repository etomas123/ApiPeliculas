using ApiPeliculas.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        // Aqui se van agregar las entidades  (Modelos)
        public DbSet<Categoria> Categorias { get; set; }    
        public DbSet<Pelicula> Pelicula { get; set; }    


    }
}
