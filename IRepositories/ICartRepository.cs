using Bookstore.Models;
using Bookstore.ViewModels;

namespace Bookstore.IRepositories {
    public interface ICartRepository :IGeneralRepsitory<Cart>{
        public Task<bool> CheckUserAndProductExist(string userId, int id);
        public Task UpdateCartQuantity(string userId, int quantity);
        public Task<int> CartNumber(string userid);
        public Task<CartViewModel> GetCurrentUserProduct(string currentUser);
        public Task<decimal> ChangeQuantityProduct(int id, string currentUser,int Quantity);
        public Task<decimal> TotalPriceCurrentUser(string userId);
        public Task<int> RemoveProductCart(int id);
    }
}
