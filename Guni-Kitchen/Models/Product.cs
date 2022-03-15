using Guni_Kitchen.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guni_Kitchen.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public string Description { get; set; }

        [Display(Name = "Price per Unit")]
        [Required]
        [Range(0.0, 500.0, ErrorMessage ="{0} has to be between Rs. {1} and Rs. {2}")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name= "Unit Of Measure")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "Size")]
        [Column(TypeName="varchar(20)")]
        public ProductSizes Size { get; set; }
        #region Navigational Properties to the Category Model

        [ForeignKey(nameof(Product.Category))]
        public short CategoryId { get; set; }

        public ProductCategory Category { get; set; }

        #endregion
    }
}
