using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models {
    public class Genre {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [MaxLength(100, ErrorMessage = "عدد الحروف اقل من 100 حرف")]
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}

