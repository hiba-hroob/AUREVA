using AUREVA.Data;
using AUREVA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AUREVA.Controllers
{
    public class ServicesController : Controller
    {
        private readonly AurevaDbContext _context;

        public ServicesController(AurevaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }

            service.CreatedAt = DateTime.UtcNow;

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service service)
        {
            if (id != service.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(service);

            var existingService = await _context.Services.FindAsync(id);

            if (existingService == null)
                return NotFound();

            existingService.Name = service.Name;
            existingService.Description = service.Description;
            existingService.Price = service.Price;
            existingService.DurationMinutes = service.DurationMinutes;
            existingService.IsActive = service.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            service.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}