using Bookstore.IRepositories;
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories {
    public class GeneralRepsitory<T> : IGeneralRepsitory<T> where T : class {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T>_dbSet;

        public GeneralRepsitory(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
          await  _dbSet.AddAsync(entity);
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
