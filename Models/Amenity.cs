using System.ComponentModel.DataAnnotations;

namespace FitHub.Models
{
    public class Amenity
    {
        [Key]
        [Required(ErrorMessage = "Amenity Id is Required")]
        [Display(Name = "Amenity Id")]
        public string AmenityID { get; set; }

        [Required(ErrorMessage = "Amenity Name is Required")]
        [StringLength(50, ErrorMessage = "Amenity Name must be at most 50 characters long")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only alphabets are allowed")]
        [Display(Name = "Amenity Name")]
        public string AmenityName { get; set; }

        [Required(ErrorMessage = "Max Capacity Per Day is Required")]
        [Range(1, 100, ErrorMessage = "The value must be between 1 and 100")]
        [Display(Name = "Max Capacity Per Day")]
        public int MaxCapacityPerDay { get; set; }

        [Required(ErrorMessage = "Cost Per Person is Required")]
        [DataType(DataType.Currency)]
        [Display(Name = "Cost Per Person")]
        public decimal CostPerPerson { get; set; }
    }
}
