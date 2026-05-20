using MoCashRoCo.Models;

namespace MoCashRoCo.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalProducts    { get; set; }
        public int TotalOrders      { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders    { get; set; }
        public int TotalCustomers   { get; set; }
        public int LowStockProducts { get; set; }
        public List<Order> RecentOrders { get; set; } = new();
    }
}
