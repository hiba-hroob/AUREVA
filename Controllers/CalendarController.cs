using AUREVA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AUREVA.Controllers
{
    public class CalendarController : Controller
    {
        private readonly AurevaDbContext _context;

        public CalendarController(AurevaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            DateTime? date,
            string? status,
            int? staffId)
        {
            var selectedDate = (date ?? DateTime.Today).Date;
            var nextDate = selectedDate.AddDays(1);

            var query = _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .Where(a =>
                    a.StartTime >= selectedDate &&
                    a.StartTime < nextDate);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            if (staffId.HasValue)
            {
                query = query.Where(a => a.StaffId == staffId.Value);
            }

            var appointments = await query
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            var staff = await _context.Staff
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewBag.SelectedDate = selectedDate;
            ViewBag.Status = status;
            ViewBag.StaffId = staffId;
            ViewBag.Staff = staff;

            return View(appointments);
        }
    }
}