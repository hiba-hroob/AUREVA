using AUREVA.Data;
using AUREVA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AUREVA.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AurevaDbContext _context;

        public ClientsController(AurevaDbContext context)
        {
            _context = context;
        }

        // GET: /Clients
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(c =>
                    c.FullName.Contains(search) ||
                    (c.Email != null && c.Email.Contains(search)) ||
                    (c.Phone != null && c.Phone.Contains(search)));
            }

            var clients = await query
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.Search = search;

            return View(clients);
        }
        // GET: /Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            client.CreatedAt = DateTime.UtcNow;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: /Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: /Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(client);
            }

            var existingClient = await _context.Clients.FindAsync(id);

            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.FullName = client.FullName;
            existingClient.Email = client.Email;
            existingClient.Phone = client.Phone;
            existingClient.DateOfBirth = client.DateOfBirth;
            existingClient.Notes = client.Notes;
            existingClient.TotalVisits = client.TotalVisits;
            existingClient.TotalSpent = client.TotalSpent;
            existingClient.FavoriteService = client.FavoriteService;
            existingClient.LastVisit = client.LastVisit;
            existingClient.Rating = client.Rating;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = existingClient.Id });
        }

        // GET: /Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: /Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}