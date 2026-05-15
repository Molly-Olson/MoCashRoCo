using System.ComponentModel.DataAnnotations;

namespace MoCashRoCo.Models
{
    public class CheckoutViewModel
    {
        [Required, MaxLength(100)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [Display(Name = "Street Address")]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string State { get; set; } = string.Empty;

        [Required, MaxLength(10)]
        [Display(Name = "ZIP Code")]
        public string Zip { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Order Notes (optional)")]
        public string? Notes { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal Subtotal => CartItems.Sum(i => i.Price * i.Quantity);
        public decimal Shipping => Subtotal >= 50 ? 0 : 7.99m;
        public decimal Total => Subtotal + Shipping;
    }
}
