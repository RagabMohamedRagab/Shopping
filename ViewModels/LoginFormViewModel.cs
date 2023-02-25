using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels {
    public class LoginFormViewModel {
        public LoginFormViewModel()
        {
            ExternalLogin = new List<AuthenticationScheme>();
        }
     
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
       
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remmber Me")]
        public bool RemmberMe { get; set; }
        public string? returnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogin { get; set; }
    }
}
