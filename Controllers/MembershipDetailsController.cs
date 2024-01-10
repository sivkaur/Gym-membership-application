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
    public class MembershipDetailsController : Controller
    {
        private readonly GymDbContext _context;

        public MembershipDetailsController(GymDbContext context)
        {
            _context = context;
        }

        // GET: MembershipDetails
        public async Task<IActionResult> Index()
        {
              return _context.MembershipDetail != null ? 
                          View(await _context.MembershipDetail.ToListAsync()) :
                          Problem("Entity set 'GymDbContext.MembershipDetail'  is null.");
        }

        // GET: MembershipDetails/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.MembershipDetail == null)
            {
                return NotFound();
            }

            var membershipDetail = await _context.MembershipDetail
                .FirstOrDefaultAsync(m => m.MembershipTypeID == id);
            if (membershipDetail == null)
            {
                return NotFound();
            }

            return View(membershipDetail);
        }

        // GET: MembershipDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MembershipDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipTypeID,MembershipTypeName,DurationMonths,Cost,Description")] MembershipDetail membershipDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membershipDetail);
        }

        // GET: MembershipDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.MembershipDetail == null)
            {
                return NotFound();
            }

            var membershipDetail = await _context.MembershipDetail.FindAsync(id);
            if (membershipDetail == null)
            {
                return NotFound();
            }
            return View(membershipDetail);
        }

        // POST: MembershipDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MembershipTypeID,MembershipTypeName,DurationMonths,Cost,Description")] MembershipDetail membershipDetail)
        {
            if (id != membershipDetail.MembershipTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membershipDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipDetailExists(membershipDetail.MembershipTypeID))
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
            return View(membershipDetail);
        }

        // GET: MembershipDetails/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.MembershipDetail == null)
            {
                return NotFound();
            }

            var membershipDetail = await _context.MembershipDetail
                .FirstOrDefaultAsync(m => m.MembershipTypeID == id);
            if (membershipDetail == null)
            {
                return NotFound();
            }

            return View(membershipDetail);
        }

        // POST: MembershipDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.MembershipDetail == null)
            {
                return Problem("Entity set 'GymDbContext.MembershipDetail'  is null.");
            }
            var membershipDetail = await _context.MembershipDetail.FindAsync(id);
            if (membershipDetail != null)
            {
                _context.MembershipDetail.Remove(membershipDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipDetailExists(string id)
        {
          return (_context.MembershipDetail?.Any(e => e.MembershipTypeID == id)).GetValueOrDefault();
        }
    }
}
