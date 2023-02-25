using Bookstore.IRepositories;
using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Bookstore.Controllers.MVC {
    public class CartController : Controller {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toast;

        public CartController(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IToastNotification toast)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _toast = toast;
        }
        [HttpGet]
        public async Task<JsonResult> AddToBasket(int id)
        {
            var currentUser = await _accountRepository.GetByEmail(User.Identity.Name);
            if (currentUser is null || id == 0)
            {
                _toast.AddSuccessToastMessage("Falied Add to Cart");
                return Json("no");
            }
            if (await _unitOfWork.ICartRepository.CheckUserAndProductExist(currentUser.Id, id))
            {
                await _unitOfWork.ICartRepository.UpdateCartQuantity(currentUser.Id, id);
            }
            else
            {
                Cart cart = new Cart()
                {
                    MovieId = id,
                    UserId = currentUser.Id,
                    Quantity = 1,
                    Price = 1
                };
                await _unitOfWork.ICartRepository.AddAsync(cart);
            }
            await _unitOfWork.Commit();
            return Json("ok");
        }
        [HttpGet]
        public async Task<JsonResult> GetNumberCart()
        {
            var currentUser = await _accountRepository.GetByEmail(User.Identity.Name);
            if (currentUser is null)
            {
                return Json("no");
            }
            return Json(await _unitOfWork.ICartRepository.CartNumber(currentUser.Id));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _accountRepository.GetByEmail(User.Identity.Name);
            if (currentUser is null)
            {
                _toast.AddWarningToastMessage("Can't Found User");
                return RedirectToAction("Index", "Movie");
            }
            var data = await _unitOfWork.ICartRepository.GetCurrentUserProduct(currentUser.Id);
            if (data != null)
                return View(data);

            _toast.AddWarningToastMessage("Can't Found User");
            return RedirectToAction("Index", "Movie");
        }
       
        [HttpGet]
        public async Task<JsonResult> RemoveProduct(int id)
        {
           if(id == 0)
            {
                return Json("no");
            }
            if(await _unitOfWork.ICartRepository.RemoveProductCart(id) > 0)
            {
                await _unitOfWork.Commit();
                var currrentuser = await _accountRepository.GetByEmail(User.Identity.Name);
                var TotalPrice =await _unitOfWork.ICartRepository.TotalPriceCurrentUser(currrentuser.Id);
                var TotalQuentity =await _unitOfWork.ICartRepository.CartNumber(currrentuser.Id);
                return Json(new { Price = TotalPrice, Quentity = TotalQuentity });
            }
            return Json("no");
        }
    }
}


