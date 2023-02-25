using Bookstore.IRepositories;
using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Bookstore.Controllers.MVC
{
    public class MovieController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toast;
        public MovieController(IUnitOfWork unitOfWork, IToastNotification toast)
        {
            _unitOfWork = unitOfWork;
            _toast = toast;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var Movies = await _unitOfWork.ImovieRepository.GetAllAsync();
            return View(Movies);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var movieVm = new MovieFormViewModel()
            {
                Genres = await _unitOfWork.IgenreRepository.GetAllAsync()
            };
            return View("MovieForm", movieVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {

            if (ModelState.IsValid)
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                return View("MovieForm", model);
            };
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                ModelState.AddModelError("Poster", "Please Add Poster..");
                return View("MovieForm", model);
            }
            var file = files.FirstOrDefault();
            IList<string> AllowExtensions = new List<string>() { ".jpg", ".png" };
            if (!AllowExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                ModelState.AddModelError("Poster", "Must be jpg,png...");
                return View("MovieForm", model);
            }
            if (file.Length > 1048576)
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                ModelState.AddModelError("Poster", "Must be jpg,png...");
                return View("MovieForm", model);

            }
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var movie = new Movie()
            {
                Title = model.Title,
                StoreLine = model.StoreLine,
                Year = model.Year,
                Rate = model.Rate,
                Poster = memoryStream.ToArray(),
                GenreId = model.GenreId
            };
            await _unitOfWork.ImovieRepository.AddAsync(movie);
            await _unitOfWork.Commit();
            _toast.AddSuccessToastMessage("Movie Create Successfully");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movie = await _unitOfWork.ImovieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var movieVM = new MovieFormViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                StoreLine = movie.StoreLine,
                Year = movie.Year,
                Rate = movie.Rate,
                Poster = movie.Poster,
                GenreId = movie.GenreId,
                Genres = await _unitOfWork.IgenreRepository.GetAllAsync(),
            };

            return View("MovieForm", movieVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                return View("MovieForm", model);
            };
            var movie = await _unitOfWork.ImovieRepository.GetByIdAsync(model.Id);
            if (movie == null)
            {
                return NotFound();
            }
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                ModelState.AddModelError("Poster", "Please Add Poster..");
                return View("MovieForm", model);
            }
            var file = files.FirstOrDefault();
            IList<string> AllowExtensions = new List<string>() { ".jpg", ".png" };
            if (!AllowExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                ModelState.AddModelError("Poster", "Must be jpg,png...");
                return View("MovieForm", model);
            }
            if (file.Length > 1048576)
            {
                model.Genres = await _unitOfWork.IgenreRepository.GetAllAsync();
                ModelState.AddModelError("Poster", "Must be jpg,png...");
                return View("MovieForm", model);

            }
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            movie.Poster = memoryStream.ToArray();
            movie.Title = model.Title;
            movie.StoreLine = model.StoreLine;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.GenreId = model.GenreId;
            await _unitOfWork.Commit();
            _toast.AddSuccessToastMessage("Movie Update Successfully");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movie = await _unitOfWork.ImovieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var movieVm = new MovieFormViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Poster = movie.Poster,
                Rate = movie.Rate,
                Year = movie.Year,
                StoreLine = movie.StoreLine,
                GenreId = movie.GenreId,
                Genres = await _unitOfWork.IgenreRepository.GetAllAsync()
            };
            return View(movieVm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movie = await _unitOfWork.ImovieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            _unitOfWork.ImovieRepository.Remove(movie);
            await _unitOfWork.Commit();
            _toast.AddSuccessToastMessage("Movie Delete Successfully");
            return RedirectToAction(nameof(Index));
        }
    }
}
