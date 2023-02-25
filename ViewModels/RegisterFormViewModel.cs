using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels {
    public class RegisterFormViewModel {
        public Guid? Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password")]
      public string ConfirmPassword { get; set; }
    }
}
