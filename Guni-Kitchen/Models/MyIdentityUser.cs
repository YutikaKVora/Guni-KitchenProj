using Guni_Kitchen.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guni_Kitchen.Models
{
    public class MyIdentityUser
    : IdentityUser<Guid>
    {

        [Display(Name = "Display Name")]
        [Required(ErrorMessage = "{0} cannot be empty.")]
        [MinLength(2, ErrorMessage = "{0} should have at least {1} characters.")]
        [StringLength(60, ErrorMessage = "{0} cannot have more than {1} characters.")]
        public string DisplayName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required]
        [PersonalData]
        [Column(TypeName = "smalldatetime")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Is Admin User?")]
        [Required]
        public bool IsAdminUser { get; set; }


        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[6789]\d{9}$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNo { get; set; }

        [Required]
        [Display(Name = "Gender")]
        [PersonalData]
        public MyIdentityGenders Gender { get; set; }
    }
}
