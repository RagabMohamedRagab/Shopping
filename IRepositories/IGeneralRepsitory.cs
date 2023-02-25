namespace Bookstore.IRepositories {
    public interface IGeneralRepsitory<T> where T:class {
        public Task AddAsync(T entity);
        public void Remove(T entity);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T>  GetByIdAsync(int? id);
    }
}
