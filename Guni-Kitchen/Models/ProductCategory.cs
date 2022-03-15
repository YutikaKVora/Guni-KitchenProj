using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guni_Kitchen.Models
{
    public class ProductCategory
    {
        [Display(Name = "Category ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short CategoryId { get; set; }


        [Required]
        [MinLength(3, ErrorMessage="{0} should have minimum {1} characters")]
        [MaxLength(30, ErrorMessage ="{0} should have more than {1} characters in length")]
        public string CategoryName { get; set; }   

        [Display(Name ="Created At")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        #region Navigational Properties to the Product Model

        public ICollection<Product> Products { get; set; }

        #endregion
    }
}
