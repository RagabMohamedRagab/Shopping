using Bookstore.IRepositories;
using Bookstore.Utilities.Services;
using Bookstore.ViewModels;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Bookstore.Controllers.MVC
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IToastNotification _toast;

        public AccountController(IToastNotification toast, IAccountRepository accountRepository)
        {
            _toast = toast;
            _accountRepository = accountRepository;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _accountRepository.EmailReadyExist(model.Email))
                {
                    if (await _accountRepository.Register(model) > 0)
                    {
                        _toast.AddSuccessToastMessage("Successfully Create");
                        return RedirectToAction("Index", "Movie");
                    }
                }
                _toast.AddErrorToastMessage("Email or Password incorrect");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.LogOut();
            _toast.AddSuccessToastMessage("Successfully LogOut");
            return RedirectToAction("Index", "Movie");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {

            var model = new LoginFormViewModel()
            {
                ExternalLogin = await _accountRepository.GetExternalAuthentication(),
                returnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _accountRepository.Login(model))
                {
                    if (!string.IsNullOrEmpty(model.returnUrl))
                    {
                        return Redirect(model.returnUrl);
                    }
                    _toast.AddSuccessToastMessage("Successfully login");
                    return RedirectToAction("Index", "Movie");
                }
            }
            _toast.AddErrorToastMessage("Email or Password incorrect");
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnurl = returnUrl });
            var poroperties = await _accountRepository.ConfigureExternalAuthentication(provider, redirectUrl);
            return new ChallengeResult(provider, poroperties);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnurl = null, string remoteError = null)
        {
            if (await _accountRepository.ExternalLogin(returnurl, remoteError) > 0)
            {
                if (string.IsNullOrEmpty(returnurl))
                {
                    _toast.AddSuccessToastMessage("Successfully login");
                    return RedirectToAction("Index", "Movie");
                }
                _toast.AddSuccessToastMessage("Successfully login");
                return Redirect(returnurl);
            }
            _toast.AddErrorToastMessage("Something Error");
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(string Email, string Password)
        {

            var user = await _accountRepository.GetByEmail(Email);
            if (user != null)
            {
                var token = await _accountRepository.GenerateTokenforPassword(user);
                var result = await _accountRepository.ResetPassword(user, token, Password);
                if (result.Succeeded)
                {
                    _toast.AddSuccessToastMessage("Successfully Change..");
                    return RedirectToAction(nameof(Login));
                }
            }
            _toast.AddErrorToastMessage("Not Found User Try Again");
            return RedirectToAction(nameof(Login));
        }

    }
}
