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
    public class AmenitiesController : Controller
    {
        private readonly GymDbContext _context;

        public AmenitiesController(GymDbContext context)
        {
            _context = context;
        }

        // GET: Amenities
        public async Task<IActionResult> Index()
        {
              return _context.Amenity != null ? 
                          View(await _context.Amenity.ToListAsync()) :
                          Problem("Entity set 'GymDbContext.Amenity'  is null.");
        }

        // GET: Amenities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Amenity == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenity
                .FirstOrDefaultAsync(m => m.AmenityID == id);
            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // GET: Amenities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Amenities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmenityID,AmenityName,MaxCapacityPerDay,CostPerPerson")] Amenity amenity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(amenity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(amenity);
        }

        // GET: Amenities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Amenity == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenity.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }
            return View(amenity);
        }

        // POST: Amenities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AmenityID,AmenityName,MaxCapacityPerDay,CostPerPerson")] Amenity amenity)
        {
            if (id != amenity.AmenityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amenity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmenityExists(amenity.AmenityID))
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
            return View(amenity);
        }

        // GET: Amenities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Amenity == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenity
                .FirstOrDefaultAsync(m => m.AmenityID == id);
            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // POST: Amenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Amenity == null)
            {
                return Problem("Entity set 'GymDbContext.Amenity'  is null.");
            }
            var amenity = await _context.Amenity.FindAsync(id);
            if (amenity != null)
            {
                _context.Amenity.Remove(amenity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmenityExists(string id)
        {
          return (_context.Amenity?.Any(e => e.AmenityID == id)).GetValueOrDefault();
        }
    }
}
