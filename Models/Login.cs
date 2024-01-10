using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitHub.Models
{
    public class Login
    {
        [Key]
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
