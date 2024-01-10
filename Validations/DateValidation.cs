using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace FitHub.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateNotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? dob = value.ToString();
            DateTime date = DateTime.Parse(dob);

            if (value != null && date > DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class DateNotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
           if (value is DateTime date)
            {
                if (date < DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

    public class DateEqualToCurrent : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? dob = value.ToString();
            DateTime date = DateTime.Parse(dob);
            if (value != null && date != DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge) { _minimumAge = minimumAge; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string? dob = value.ToString();
                DateTime DOB = DateTime.Parse(dob);
                int age = DateTime.Now.Year - DOB.Year;
                if (DateTime.Now.Month < DOB.Month || 
                    (DateTime.Now.Month == DOB.Month && DateTime.Now.Day < DOB.Day))
                {
                    age--;
                }

                if (age < _minimumAge)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }

}
