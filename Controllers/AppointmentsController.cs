using AUREVA.Data;
using AUREVA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AUREVA.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AurevaDbContext _context;

        public AppointmentsController(AurevaDbContext context)
        {
            _context = context;
        }

        // GET: /Appointments

        public async Task<IActionResult> Index()
        {
            var palestineTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById("Asia/Hebron");

            var palestineNow =
                TimeZoneInfo.ConvertTime(
                    DateTimeOffset.UtcNow,
                    palestineTimeZone);

            var localToday = palestineNow.Date;
            var localTomorrow = localToday.AddDays(1);

            var today = new DateTimeOffset(
                localToday,
                palestineTimeZone.GetUtcOffset(localToday));

            var tomorrow = new DateTimeOffset(
                localTomorrow,
                palestineTimeZone.GetUtcOffset(localTomorrow));

            var todayUtc = today.UtcDateTime;
            var tomorrowUtc = tomorrow.UtcDateTime;

            var todayAppointments = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .Where(a =>
                    a.StartTime >= todayUtc &&
                    a.StartTime < tomorrowUtc)
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

            return View(todayAppointments);
        }

        // GET: /Appointments/Create
        public async Task<IActionResult> Create()
        {
            await LoadFormData();
            return View();
        }

        // POST: /Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                await LoadFormData(appointment);
                return View(appointment);
            }

            var palestineTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("Asia/Hebron");

            var localStart = DateTime.SpecifyKind(
                appointment.StartTime,
                DateTimeKind.Unspecified);

            appointment.StartTime = TimeZoneInfo.ConvertTimeToUtc(
                localStart,
                palestineTimeZone);

            var start = appointment.StartTime;
            var end = start.AddMinutes(appointment.DurationMinutes);



            var conflict = await _context.Appointments
                .Where(a =>
                    a.Status != "Cancelled" &&
                    (
                        a.StaffId == appointment.StaffId ||
                        a.ClientId == appointment.ClientId
                    ))
                .AnyAsync(a =>
                    start < a.StartTime.AddMinutes(a.DurationMinutes) &&
                    end > a.StartTime
                );

            if (conflict)
            {
                ModelState.AddModelError(
                    "",
                    "This appointment conflicts with an existing appointment for the selected client or staff member."
                );

                await LoadFormData(appointment);
                return View(appointment);
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: /Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: /Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            await LoadFormData(appointment);

            return View(appointment);
        }

        // POST: /Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadFormData(appointment);
                return View(appointment);
            }

            var existingAppointment = await _context.Appointments
                .FindAsync(id);

            if (existingAppointment == null)
            {
                return NotFound();
            }

            var palestineTimeZone =
       TimeZoneInfo.FindSystemTimeZoneById("Asia/Hebron");

            var localStart = DateTime.SpecifyKind(
                appointment.StartTime,
                DateTimeKind.Unspecified);

            appointment.StartTime = TimeZoneInfo.ConvertTimeToUtc(
                localStart,
                palestineTimeZone);

            var start = appointment.StartTime;
            var end = start.AddMinutes(appointment.DurationMinutes);


            var conflict = await _context.Appointments
                .Where(a =>
                    a.Id != id &&
                    a.Status != "Cancelled" &&
                    (
                        a.StaffId == appointment.StaffId ||
                        a.ClientId == appointment.ClientId
                    ))
                .AnyAsync(a =>
                    start < a.StartTime.AddMinutes(a.DurationMinutes) &&
                    end > a.StartTime
                );

            if (conflict)
            {
                ModelState.AddModelError(
                    "",
                    "This appointment conflicts with another appointment for the selected client or staff member."
                );

                await LoadFormData(appointment);
                return View(appointment);
            }

            existingAppointment.StartTime = appointment.StartTime;
            existingAppointment.DurationMinutes = appointment.DurationMinutes;
            existingAppointment.Status = appointment.Status;
            existingAppointment.Notes = appointment.Notes;
            existingAppointment.ClientId = appointment.ClientId;
            existingAppointment.ServiceId = appointment.ServiceId;
            existingAppointment.StaffId = appointment.StaffId;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = existingAppointment.Id }
            );
        }

        // GET: /Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: /Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment =
                await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadFormData(
            Appointment? appointment = null)
        {
            var clients = await _context.Clients
                .OrderBy(c => c.FullName)
                .ToListAsync();

            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            var staff = await _context.Staff
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewBag.Clients = new SelectList(
                clients,
                "Id",
                "FullName",
                appointment?.ClientId
            );

            ViewBag.Services = new SelectList(
                services,
                "Id",
                "Name",
                appointment?.ServiceId
            );

            ViewBag.Staff = new SelectList(
                staff,
                "Id",
                "FullName",
                appointment?.StaffId
            );
        }
    }
}

