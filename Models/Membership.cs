using System;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitHub.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace FitHub.Models
{
    public class Membership
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MembershipID { get; set; }

        [ForeignKey("User")]
        [HiddenInput(DisplayValue = true)]
        public string UserID { get; set; }

        [ForeignKey("MD")]
        [DisplayName("Membership Type")]
        [Required]
        public string MembershipTypeID { get; set; }

        [Required]
        [DateNotInPastAttribute(ErrorMessage = 
            "The date must be greater than or equal to the current date.")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [HiddenInput(DisplayValue = true)]
        [Display(Name = "End Date"), DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
       
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }
        

        public virtual MembershipDetail MD { get; set; }
        public virtual User User { get; set; }
    }
}
