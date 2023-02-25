using Bookstore.IRepositories;
using Bookstore.Models;

namespace Bookstore.Repositories {
    public class UnitOfWork : IUnitOfWork {
        private readonly ApplicationDbContext _context;
        public IMovieRepository ImovieRepository { get; set; }
        public IGenreRepository IgenreRepository { get; set; }

        public ICartRepository ICartRepository { get; set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ImovieRepository = new MovieRepository(context);
            IgenreRepository= new GenreRepository(context);
            ICartRepository=new CartRepository(context);
        
        }

        public async Task<int> Commit()
        {
           return  await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}



