namespace Bookstore.ViewModels {
    public class CartViewModel {
        public virtual IEnumerable<MovieViewModel> Movies { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
