using Bookstore.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers.API {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GenresController : ControllerBase {
        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Genres=await _unitOfWork.IgenreRepository.GetAllAsync();
           return Genres is null ? NotFound() : Ok(Genres);
        }
    }
}
