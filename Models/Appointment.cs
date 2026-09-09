using System.ComponentModel.DataAnnotations;

namespace AUREVA.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public int DurationMinutes { get; set; }

        public string Status { get; set; } = "Confirmed";

        public string? Notes { get; set; }

        // Client
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        // Service
        public int ServiceId { get; set; }

        public Service? Service { get; set; }

        // Staff / Artist
        public int StaffId { get; set; }

        public Staff? Staff { get; set; }
    }
}