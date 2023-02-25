using Bookstore.IRepositories;
using Bookstore.Models;

namespace Bookstore.Repositories {
    public class GenreRepository : GeneralRepsitory<Genre>, IGenreRepository {
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
