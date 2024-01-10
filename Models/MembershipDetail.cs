using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FitHub.Models
{
    public class MembershipDetail
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MembershipTypeID { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Membership Type")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only alphabets are allowed")]
        public string MembershipTypeName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field must be greater than 1.")]
        [Display(Name = "Duration (in months)")] 
        public int DurationMonths { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set;}

        [Required]
        public string Description { get; set;}
    }
}
