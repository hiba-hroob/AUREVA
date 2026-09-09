using System.ComponentModel.DataAnnotations;

namespace AUREVA.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Notes { get; set; }

        public int TotalVisits { get; set; }

        public decimal TotalSpent { get; set; }

        [StringLength(100)]
        public string? FavoriteService { get; set; }

        public DateTime? LastVisit { get; set; }

        public decimal? Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}