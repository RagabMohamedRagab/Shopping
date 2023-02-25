using Bookstore.IRepositories;
using Bookstore.Models;

namespace Bookstore.Repositories {
    public class MovieRepository : GeneralRepsitory<Movie>, IMovieRepository {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task UpdateAsync(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
