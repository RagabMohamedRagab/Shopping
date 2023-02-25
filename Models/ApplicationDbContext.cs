using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models {
    public class ApplicationDbContext:IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){ }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
    }
}
