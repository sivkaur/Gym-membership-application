using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitHub.Data;
using FitHub.Models;
using Microsoft.AspNetCore.Authorization;

namespace FitHub.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class SpasController : Controller
    {
        private readonly GymDbContext _context;

        public SpasController(GymDbContext context)
        {
            _context = context;
        }

        // GET: Spas
        public async Task<IActionResult> Index()
        {
              return _context.Spa != null ? 
                          View(await _context.Spa.ToListAsync()) :
                          Problem("Entity set 'GymDbContext.Spa'  is null.");
        }

        // GET: Spas/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null || _context.Spa == null)
            {
                return NotFound();
            }

            var spa = await _context.Spa
                .FirstOrDefaultAsync(m => m.Date == id);
            if (spa == null)
            {
                return NotFound();
            }

            return View(spa);
        }

        // GET: Spas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,AmenityId,NumberReserved")] Spa spa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spa);
        }

        // GET: Spas/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null || _context.Spa == null)
            {
                return NotFound();
            }

            var spa = await _context.Spa.FindAsync(id);
            if (spa == null)
            {
                return NotFound();
            }
            return View(spa);
        }

        // POST: Spas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("Date,AmenityId,NumberReserved")] Spa spa)
        {
            if (id != spa.Date)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpaExists(spa.Date))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(spa);
        }

        // GET: Spas/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null || _context.Spa == null)
            {
                return NotFound();
            }

            var spa = await _context.Spa
                .FirstOrDefaultAsync(m => m.Date == id);
            if (spa == null)
            {
                return NotFound();
            }

            return View(spa);
        }

        // POST: Spas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            if (_context.Spa == null)
            {
                return Problem("Entity set 'GymDbContext.Spa'  is null.");
            }
            var spa = await _context.Spa.FindAsync(id);
            if (spa != null)
            {
                _context.Spa.Remove(spa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpaExists(DateTime id)
        {
          return (_context.Spa?.Any(e => e.Date == id)).GetValueOrDefault();
        }
    }
}
