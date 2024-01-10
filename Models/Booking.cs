using FitHub.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using FitHub.Data;

namespace FitHub.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? BookingID { get; set; }

        [ForeignKey("User")]
        [Display(Name = "User Id")]
        public string? UserID { get; set; }

        [ForeignKey("Amenity")]
        [Display(Name = "Amenity Id")]
        public string? AmenityID { get; set; }

        [Required(ErrorMessage = "Booking Date is Required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        [DateNotInPastAttribute(ErrorMessage = "The date must be greater than or equal to the current date.")]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Number Of People is Required")]
        [Display(Name = "Number Of People")]
        public int NumberOfPeople { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        [Display(Name = "Purchased Date")]
        public DateTime PurchasedDate { get; set; }

        public virtual User? User { get; set; }
        public virtual Amenity? Amenity { get; set; }
    }
}


