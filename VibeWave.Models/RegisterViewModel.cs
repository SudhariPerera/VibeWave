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
    public class RegisterViewModel
    {
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name ="Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100,ErrorMessage ="The password must be between 6 and 100 characters long.",MinimumLength =6)]
        [DataType(DataType.Password)]//to hide the real password into another form. Can protect the real one.
        [Display(Name ="Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]//to compare the confirm one is same or not with privious one.
        public string ConfirmPassword { get; set; }

    }
}
