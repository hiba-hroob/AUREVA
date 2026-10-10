using AUREVA.Data;
using AUREVA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AUREVA.Controllers
{
    public class HomeController : Controller
    {
        private readonly AurevaDbContext _context;

        public HomeController(AurevaDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            // توقيت فلسطين
            var palestineTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById("Asia/Hebron");

            var palestineNow =
                TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow,
                    palestineTimeZone);

            var localToday = palestineNow.Date;
            var localTomorrow = localToday.AddDays(1);

            // تحويل بداية ونهاية اليوم المحلي إلى UTC
            var today = TimeZoneInfo.ConvertTimeToUtc(
                DateTime.SpecifyKind(
                    localToday,
                    DateTimeKind.Unspecified),
                palestineTimeZone);

            var tomorrow = TimeZoneInfo.ConvertTimeToUtc(
                DateTime.SpecifyKind(
                    localTomorrow,
                    DateTimeKind.Unspecified),
                palestineTimeZone);

            var todayAppointments = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .Where(a =>
                    a.StartTime >= today &&
                    a.StartTime < tomorrow)
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            var staff = await _context.Staff
                .OrderBy(s => s.FullName)
                .ToListAsync();

            var todayRevenue = todayAppointments
                .Where(a =>
                    a.Status != "Cancelled" &&
                    a.Service != null)
                .Sum(a => a.Service!.Price);

            ViewBag.Today = localToday;
            ViewBag.TodayAppointments = todayAppointments;
            ViewBag.Staff = staff;
            ViewBag.TodayRevenue = todayRevenue;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
                });
        }
    }
}