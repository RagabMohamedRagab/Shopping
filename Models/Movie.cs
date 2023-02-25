using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models {
    public class Movie {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(250, ErrorMessage = "عدد الحروف اقل من 250 حرف")]
        public string StoreLine { get; set; }
        public byte[] Poster { get; set; }
        public byte? GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

    }
}
