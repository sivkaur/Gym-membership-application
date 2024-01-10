using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FitHub.Validations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitHub.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? UserID { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only alphabets are allowed")]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only alphabets are allowed")]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression("^[2-9]\\d{9}$", ErrorMessage = "Invalid Phone Number")]
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is Required")]
        [DisplayName("DOB")]
        [DateNotInFuture(ErrorMessage = "DOB cannot be in Future")]
        [MinimumAge(16, ErrorMessage = "You must be at least 16 years old")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        public string? DOB { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Street Address is Required")]
        [DisplayName("Street Address")]
        [RegularExpression("^[a-zA-Z0-9 -]*$", ErrorMessage = "Invalid Street Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only alphabets are allowed")]
        [DisplayName("City")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Province or State is Required")]
        [DisplayName("Province/State")]
        public string? Province { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Postal Code is Required")]
        [DisplayName("Postal/Zip Code")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d|\d{5}(-\d{4})?$", 
            ErrorMessage = "Invalid Postal/Zip Code")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", 
            ErrorMessage = "Invalid Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DisplayName("Confirm Password")]
        [NotMapped]
        public string? ConfirmPassword { get; set; }

        [DefaultValue(false)]
        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

    }
}
