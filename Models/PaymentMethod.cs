using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FitHub.Models
{
    public class PaymentMethod : IValidatableObject
    {
        [Required(ErrorMessage = "Card Type is Required")]
        [DisplayName("Card Type")]
        public string? CardType { get; set; }


        [Required(ErrorMessage = "Name on Card is Required")]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50 characters")]
        [RegularExpression("^([a-zA-Z] *)*$", ErrorMessage = "Name on Card should contain only alphabets")]
        [DisplayName("Name on Card")]
        public string? NameOnCard { get; set; }

        [Required(ErrorMessage = "Card Number is Required")]
        [DisplayName("Credit Card Number")]
        [CreditCard(ErrorMessage = "Invalid Credit Card Number")]
        public string? CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry Date is Required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$",
            ErrorMessage = "Format should be MM/YYYY")]
        [ExpiryValidation]
        [DisplayName("Expiry Date")]
        public string? ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is Required")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Invalid CVV")]
        public string? CVV { get; set; }
        public Booking? Booking { get; set; }
        public Membership? Membership{ get; set; }
       
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var regExpression = @"^4[0-9]{15}|5[1-5][0-9]{14}|3[47][0-9]{13}$";

            if (CardNumber == null || !Regex.IsMatch(CardNumber, regExpression))
            {
                yield return new ValidationResult("Invalid Card Number", new[] { nameof(CardNumber) });
            }

            if (!ValidateCardbyType())
            {
                yield return new ValidationResult("Invalid Card Number for the selected card type", new[] { nameof(CardNumber) });
            }
        }

        private bool ValidateCardbyType()
        {

            if (CardNumber == null)
            {
                return false;
            }

            string? VisaRegex = @"^4[0-9]{15}$";
            string? MasterCardRegex = @"^5[1-5][0-9]{14}$";
            string? AmexRegex = @"^3[47][0-9]{13}$";

            switch (CardType?.ToLower().Replace(" ", ""))
            {
                case "visa":
                    return Regex.IsMatch(CardNumber, VisaRegex);
                case "mastercard":
                    return Regex.IsMatch(CardNumber, MasterCardRegex);
                case "americanexpress":
                    return Regex.IsMatch(CardNumber, AmexRegex);

                default:
                    return false;
            }
        }
    }

    // VALIDATION for Expiry Date of the Card

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExpiryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? expiry = value?.ToString();
            _ = int.TryParse(expiry?[..2], out int expiryMonth);
            _ = int.TryParse(expiry?.Substring(3, 4), out int expiryYear);

            if (expiryMonth < 1 || expiryMonth > 12)
            {
                return new ValidationResult(ErrorMessage = "Month can only be between 01 to 12");
            }

            if (expiryYear < 2016 || expiryYear > 2031)
            {
                return new ValidationResult(ErrorMessage = "Year can only be between 2016 and 2031");
            }

            if (expiryMonth < DateTime.Now.Month && expiryYear < DateTime.Now.Year)
            {
                return new ValidationResult(ErrorMessage = "Card is Expired");
            }

            return ValidationResult.Success;
        }
    }
}