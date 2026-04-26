using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoCashRoCo.Models
{
    public enum OrderStatus { Pending = 0, Processing = 1, Shipped = 2, Complete = 3, Cancelled = 4 }

    public class Order
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        [Required, MaxLength(200)]
        public string ContactName { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string ContactEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}