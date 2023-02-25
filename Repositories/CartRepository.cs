using Bookstore.IRepositories;
using Bookstore.Models;
using Bookstore.ViewModels;

namespace Bookstore.Repositories {
    public class CartRepository : GeneralRepsitory<Cart>, ICartRepository {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<int> CartNumber(string userid)
        {
            var Carts = await GetAllAsync();
            return Carts.Where(user => user.UserId == userid).ToList().Sum(b=>b.Quantity);
        }
        public async Task<bool> CheckUserAndProductExist(string userId, int id)
        {
            var Carts = await GetAllAsync();
            return Carts.Any(b => b.UserId == userId && b.MovieId == id);
        }

        public async Task UpdateCartQuantity(string userId, int id)
        {
            var Carts = await GetAllAsync();
            var Increment = Carts.SingleOrDefault(cart => cart.MovieId == id && cart.UserId == userId);
            Increment.Quantity += 1;
            Increment.Price += 1;
        }
        public async Task<CartViewModel> GetCurrentUserProduct(string currentUser)
        {
            var CartProducts = await GetAllAsync();
            var CurrentUserProduct = CartProducts.Where(user => user.UserId == currentUser);
            if (CurrentUserProduct is null)
            {
                return null;
            }
            IList<MovieViewModel> Movies = new List<MovieViewModel>();
            decimal TotalPrice = 0;
            foreach (var movie in CurrentUserProduct.ToList())
            {
                MovieViewModel movieForm = new MovieViewModel()
                {
                    Id=movie.Id,
                    Title = movie.Movie.Title,
                    Img = movie.Movie.Poster,
                    Quantity = movie.Quantity,
                    Price = movie.Price,
                };
                TotalPrice += movie.Price;
                Movies.Add(movieForm);

            }
            CartViewModel cartView = new CartViewModel();
            cartView.Movies = Movies;
            cartView.TotalPrice = TotalPrice;
            return cartView;
        }
        public async Task<decimal> ChangeQuantityProduct(int id, string currentUser, int Quantity)
        {
            var Carts =await GetAllAsync();
            var CurrentUserProduct = Carts.SingleOrDefault(b => b.UserId == currentUser && b.Id == id);
            if (CurrentUserProduct is null)
                return -1;
            CurrentUserProduct.Quantity=Quantity;
            CurrentUserProduct.Price = Quantity * 1; // Let 1 is price for all Product
            return CurrentUserProduct.Price; 
        }
        public async Task<decimal> TotalPriceCurrentUser(string userId)
        {
            var Carts =await GetAllAsync();
            var total = Carts.Where(b => b.UserId == userId).Sum(b => b.Price);
            return total;
        }
        public async Task<int> RemoveProductCart(int id)
        {
            var cart =await GetByIdAsync(id);
            if (cart is null)
            {
                return -1;
            }
            Remove(cart);
            return 1;
        }
    }
}


