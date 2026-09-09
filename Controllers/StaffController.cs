using AUREVA.Data;
using AUREVA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AUREVA.Controllers
{
    public class StaffController : Controller
    {
        private readonly AurevaDbContext _context;

        public StaffController(AurevaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var staff = await _context.Staff
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return View(staff);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return View(staff);
            }

            staff.CreatedAt = DateTime.UtcNow;

            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.Id == id);

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(staff);
            }

            var existingStaff = await _context.Staff.FindAsync(id);

            if (existingStaff == null)
            {
                return NotFound();
            }

            existingStaff.FullName = staff.FullName;
            existingStaff.Role = staff.Role;
            existingStaff.Email = staff.Email;
            existingStaff.Phone = staff.Phone;
            existingStaff.IsAvailable = staff.IsAvailable;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = existingStaff.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.Id == id);

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}