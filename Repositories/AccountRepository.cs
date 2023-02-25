using Bookstore.IRepositories;
using Bookstore.Models;
using Bookstore.Utilities.Services;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Security.Policy;

namespace Bookstore.Repositories {
    public class AccountRepository :IAccountRepository {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailingService _IMailingService;
        public AccountRepository(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMailingService iMailingService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _IMailingService = iMailingService;
        }

        public async Task<bool> EmailReadyExist(string email)
        {
            return (await _userManager.FindByEmailAsync(email) is null) ? false : true;
        }

        public async Task<IList<AuthenticationScheme>> GetExternalAuthentication()
        {
            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<bool> Login(LoginFormViewModel model)
        {
            SignInResult result =await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RemmberMe, true);
            return result.Succeeded ? true : false;
        }
        public async Task<AuthenticationProperties> ConfigureExternalAuthentication(string provider, string redirect)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
        }
        public async Task<AppUser> GetByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<string> GenerateTokenforPassword(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<IdentityResult> ResetPassword(AppUser user, string Token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, Token, newPassword);
        }
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<int> Register(RegisterFormViewModel model)
        {
            try
            {
                AppUser user = new AppUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    PhoneNumber = model.Phone,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return -1;
                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                await _IMailingService.SendEmailAsync(model.Email, "Confirm Email", "SuccessFully Register");
                return 1;

            }
            catch (Exception)
            {

                return -1;
            }
        }
        public async Task<int> ExternalLogin(string returnurl = null, string remoteError = null)
        {
            returnurl = returnurl ?? "~/";
            LoginFormViewModel model = new LoginFormViewModel()
            {
                ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };
            if (remoteError != null)
            {
                return -1;
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return -1;
            }
            var signInUser = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInUser.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                await _IMailingService.SendEmailAsync(email, "Confirm Email", "SuccessFully Register");
                return 1;
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new AppUser()
                        {
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _IMailingService.SendEmailAsync(user.Email, "Confirm Email", "SuccessFully Register");
                    return 1;
                }
                return -1;
            }
        }
    }
}
