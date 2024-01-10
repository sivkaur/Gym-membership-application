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
    public class SaunasController : Controller
    {
        private readonly GymDbContext _context;

        public SaunasController(GymDbContext context)
        {
            _context = context;
        }

        // GET: Saunas
        public async Task<IActionResult> Index()
        {
              return _context.Sauna != null ? 
                          View(await _context.Sauna.ToListAsync()) :
                          Problem("Entity set 'GymDbContext.Sauna'  is null.");
        }

        // GET: Saunas/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null || _context.Sauna == null)
            {
                return NotFound();
            }

            var sauna = await _context.Sauna
                .FirstOrDefaultAsync(m => m.Date == id);
            if (sauna == null)
            {
                return NotFound();
            }

            return View(sauna);
        }

        // GET: Saunas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Saunas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,AmenityId,NumberReserved")] Sauna sauna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sauna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sauna);
        }

        // GET: Saunas/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null || _context.Sauna == null)
            {
                return NotFound();
            }

            var sauna = await _context.Sauna.FindAsync(id);
            if (sauna == null)
            {
                return NotFound();
            }
            return View(sauna);
        }

        // POST: Saunas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("Date,AmenityId,NumberReserved")] Sauna sauna)
        {
            if (id != sauna.Date)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sauna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaunaExists(sauna.Date))
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
            return View(sauna);
        }

        // GET: Saunas/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null || _context.Sauna == null)
            {
                return NotFound();
            }

            var sauna = await _context.Sauna
                .FirstOrDefaultAsync(m => m.Date == id);
            if (sauna == null)
            {
                return NotFound();
            }

            return View(sauna);
        }

        // POST: Saunas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            if (_context.Sauna == null)
            {
                return Problem("Entity set 'GymDbContext.Sauna'  is null.");
            }
            var sauna = await _context.Sauna.FindAsync(id);
            if (sauna != null)
            {
                _context.Sauna.Remove(sauna);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaunaExists(DateTime id)
        {
          return (_context.Sauna?.Any(e => e.Date == id)).GetValueOrDefault();
        }
    }
}
