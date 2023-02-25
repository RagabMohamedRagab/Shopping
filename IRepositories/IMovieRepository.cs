using Bookstore.Models;

namespace Bookstore.IRepositories {
    public interface IMovieRepository:IGeneralRepsitory<Movie> {
        public Task UpdateAsync(Movie movie);
    }
}
