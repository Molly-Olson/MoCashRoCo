using System.ComponentModel.DataAnnotations;

namespace MoCashRoCo.Models
{
    public class SiteSettings
    {
        public int SiteSettingsId { get; set; }
        [Required, MaxLength(200)]
        public string BusinessName { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Tagline { get; set; }
        [MaxLength(500)]
        public string? LogoUrl { get; set; }
        [MaxLength(200)]
        public string? ContactEmail { get; set; }
        [MaxLength(20)]
        public string? ContactPhone { get; set; }
        [MaxLength(500)]
        public string? Address { get; set; }
        [MaxLength(7)]
        public string PrimaryColor { get; set; } = "#2e7d32";
        [MaxLength(7)]
        public string AccentColor { get; set; } = "#ff6f00";
    }
}