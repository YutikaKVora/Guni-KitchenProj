﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

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


        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[6789]\d{9}$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNo { get; set; }
    }
}