using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models {
    public class Cart {
        public int Id { get; set; }
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey(nameof(MovieId))]
        public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public int Quantity { get; set; } = 0;
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; } 
    }
}
