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
    public class SwimmingPoolsController : Controller
    {
        private readonly GymDbContext _context;

        public SwimmingPoolsController(GymDbContext context)
        {
            _context = context;
        }

        // GET: SwimmingPools
        public async Task<IActionResult> Index()
        {
              return _context.SwimmingPool != null ? 
                          View(await _context.SwimmingPool.ToListAsync()) :
                          Problem("Entity set 'GymDbContext.SwimmingPool'  is null.");
        }

        // GET: SwimmingPools/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null || _context.SwimmingPool == null)
            {
                return NotFound();
            }

            var swimmingPool = await _context.SwimmingPool
                .FirstOrDefaultAsync(m => m.Date == id);
            if (swimmingPool == null)
            {
                return NotFound();
            }

            return View(swimmingPool);
        }

        // GET: SwimmingPools/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SwimmingPools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,AmenityId,NumberReserved")] SwimmingPool swimmingPool)
        {
            if (ModelState.IsValid)
            {
                _context.Add(swimmingPool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(swimmingPool);
        }

        // GET: SwimmingPools/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null || _context.SwimmingPool == null)
            {
                return NotFound();
            }

            var swimmingPool = await _context.SwimmingPool.FindAsync(id);
            if (swimmingPool == null)
            {
                return NotFound();
            }
            return View(swimmingPool);
        }

        // POST: SwimmingPools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("Date,AmenityId,NumberReserved")] SwimmingPool swimmingPool)
        {
            if (id != swimmingPool.Date)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(swimmingPool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SwimmingPoolExists(swimmingPool.Date))
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
            return View(swimmingPool);
        }

        // GET: SwimmingPools/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null || _context.SwimmingPool == null)
            {
                return NotFound();
            }

            var swimmingPool = await _context.SwimmingPool
                .FirstOrDefaultAsync(m => m.Date == id);
            if (swimmingPool == null)
            {
                return NotFound();
            }

            return View(swimmingPool);
        }

        // POST: SwimmingPools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            if (_context.SwimmingPool == null)
            {
                return Problem("Entity set 'GymDbContext.SwimmingPool'  is null.");
            }
            var swimmingPool = await _context.SwimmingPool.FindAsync(id);
            if (swimmingPool != null)
            {
                _context.SwimmingPool.Remove(swimmingPool);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SwimmingPoolExists(DateTime id)
        {
          return (_context.SwimmingPool?.Any(e => e.Date == id)).GetValueOrDefault();
        }
    }
}
