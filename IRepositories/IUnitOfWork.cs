namespace Bookstore.IRepositories {
    public interface IUnitOfWork:IDisposable {
        public IMovieRepository ImovieRepository { get; set; }
        public IGenreRepository IgenreRepository { get; set; }
        public ICartRepository ICartRepository { get; set; }

        public Task<int> Commit();
    }
}
