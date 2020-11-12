using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords can't match!")]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
