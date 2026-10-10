
using AUREVA.Models;

namespace AUREVA.Data
{
    public static class DemoData
    {
        public static List<Service> GetServices()
        {
            return new List<Service>
            {
                new Service
                {
                    Id = 1,
                    Name = "Signature Blowout",
                    Price = 35.00m,
                    DurationMinutes = 45,
                    IsActive = true
                },
                new Service
                {
                    Id = 3,
                    Name = "Manicure Ritual",
                    Price = 30.00m,
                    DurationMinutes = 57,
                    IsActive = false
                },
                new Service
                {
                    Id = 4,
                    Name = "Manicure Ritual",
                    Price = 55.00m,
                    DurationMinutes = 74,
                    IsActive = true
                },
                new Service
                {
                    Id = 5,
                    Name = "Natural Makeup",
                    Price = 140.00m,
                    DurationMinutes = 60,
                    IsActive = true
                }
            };
        }

        public static List<Staff> GetStaff()
        {
            return new List<Staff>
            {
                new Staff
                {
                    Id = 1,
                    FullName = "Maya",
                    Role = "Nails Artist",
                    IsAvailable = true
                },
                new Staff
                {
                    Id = 2,
                    FullName = "Lina",
                    Role = "Nail Artist",
                    IsAvailable = true
                },
                new Staff
                {
                    Id = 4,
                    FullName = "Raya",
                    Role = "Hair Artist",
                    IsAvailable = true
                }
            };
        }
    }
}