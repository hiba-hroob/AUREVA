using AUREVA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AUREVA.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AurevaDbContext _context;

        public DashboardController(AurevaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);

            var monthStart = new DateTime(today.Year, today.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            var appointments = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .Where(a => a.Status != "Cancelled")
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            var todayAppointments = appointments
                .Where(a => a.StartTime >= today && a.StartTime < tomorrow)
                .OrderBy(a => a.StartTime)
                .ToList();



            var upcomingAppointments = appointments
                .Where(a => a.StartTime >= DateTime.Now)
                .OrderBy(a => a.StartTime)
                .Take(6)
                .ToList();

            var reminderAppointments = upcomingAppointments
    .Where(a =>
        a.StartTime >= DateTime.Now &&
        a.StartTime <= DateTime.Now.AddHours(24))
    .OrderBy(a => a.StartTime)
    .ToList();

            var todayRevenue = todayAppointments
                .Where(a => a.Service != null)
                .Sum(a => a.Service!.Price);

            var weekRevenue = appointments
                .Where(a => a.StartTime >= weekStart && a.StartTime < weekEnd)
                .Where(a => a.Service != null)
                .Sum(a => a.Service!.Price);

            var monthRevenue = appointments
                .Where(a => a.StartTime >= monthStart && a.StartTime < monthEnd)
                .Where(a => a.Service != null)
                .Sum(a => a.Service!.Price);

            var topServices = appointments
                .Where(a => a.Service != null)
                .GroupBy(a => a.Service!.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(a => a.Service!.Price)
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            var totalClients = await _context.Clients.CountAsync();

            var activeServices = await _context.Services
                .CountAsync(s => s.IsActive);

            var availableStaff = await _context.Staff
                .CountAsync(s => s.IsAvailable);

            ViewBag.TotalClients = totalClients;
            ViewBag.TodayAppointments = todayAppointments;
            ViewBag.UpcomingAppointments = upcomingAppointments;
            ViewBag.ReminderAppointments = reminderAppointments;
            ViewBag.TodayRevenue = todayRevenue;
            ViewBag.WeekRevenue = weekRevenue;
            ViewBag.MonthRevenue = monthRevenue;
            ViewBag.ActiveServices = activeServices;
            ViewBag.AvailableStaff = availableStaff;
            ViewBag.TopServices = topServices;

            return View();
        }
    }
}