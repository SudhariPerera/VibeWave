using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace VibeWave.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]//to hide the real password into another form. Can protect the real one.
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

    }
}
