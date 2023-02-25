using Microsoft.AspNetCore.Identity;

namespace Bookstore.Models {
    public class AppUser:IdentityUser {
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
