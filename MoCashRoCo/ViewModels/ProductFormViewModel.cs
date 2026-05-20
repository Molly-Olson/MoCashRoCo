using System.ComponentModel.DataAnnotations;

namespace MoCashRoCo.ViewModels
{
    public class ProductFormViewModel
    {
        public int ProductId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required, Range(0.01, 99999.99)]
        public decimal Price { get; set; }

        [MaxLength(500), Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Required, Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required, Range(0, 100000), Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Display(Name = "Active (visible in store)")]
        public bool IsActive { get; set; } = true;
    }
}
