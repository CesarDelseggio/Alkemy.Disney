
using Alkemy.Disney.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alkemy.Disney.Models
{
    public class DisneyDbContext : IdentityDbContext
    {
        public DisneyDbContext(DbContextOptions<DisneyDbContext> options)
            : base(options)
        {
            //base.Database.EnsureDeleted();
            base.Database.EnsureCreated();
        }

        public DbSet<Caracter> Caracters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
