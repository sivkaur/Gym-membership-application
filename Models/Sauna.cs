using System.ComponentModel.DataAnnotations;

namespace FitHub.Models
{
    public class Sauna
    {
        [Key]
        [Required(ErrorMessage = "Date is required")]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in the format 'YYYY-MM-DD'.")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Amenity Id is required")]
        [Display(Name = "Amenity Id")]
        public int AmenityId { get; set; }

        [Required(ErrorMessage = "Number of Reserved Slots is required")]
        [Display(Name = "Number of Reserved Slots")]
        public int NumberReserved { get; set; }

        public Sauna()
        {
            AmenityId = 2;
        }
    }
}
