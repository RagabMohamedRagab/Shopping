using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.IRepositories {
    public interface IAccountRepository {
        public Task<int> Register(RegisterFormViewModel user);
        public Task LogOut ();
        public Task<AppUser> GetByEmail(string email);
        public Task<string> GenerateTokenforPassword(AppUser user);
        public Task<IdentityResult> ResetPassword(AppUser user,string Token,string newPassword);
        public Task<bool> EmailReadyExist(string email);
        public Task<IList<AuthenticationScheme>> GetExternalAuthentication();
        public Task<bool> Login(LoginFormViewModel model);
        public Task<AuthenticationProperties> ConfigureExternalAuthentication(string provider,string redirect);
        public Task<int> ExternalLogin(string returnurl = null, string remoteError = null);

    }
}
