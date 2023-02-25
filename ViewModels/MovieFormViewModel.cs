using Bookstore.Models;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels {
    public class MovieFormViewModel {
        public int Id { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        [Range(1,10)]
        public double Rate { get; set; }
        [StringLength(250, ErrorMessage = "عدد الحروف اقل من 100 حرف")]
        public string StoreLine { get; set; }
        public byte[] Poster { get; set; }
        [Display(Name ="Genre")]
        public byte? GenreId { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
