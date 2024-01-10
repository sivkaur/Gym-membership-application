using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitHub.Data;
using FitHub.Models;
using FitHub.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace FitHub.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly GymDbContext _context;
        private readonly AmenityManagementService _amenityManagementService;

        public BookingsController(GymDbContext context, AmenityManagementService amenityManagementService)
        {
            _context = context;
            _amenityManagementService = amenityManagementService;
        }

        // GET: Bookings

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserID").Value;
            var bookings = _context.Booking
                        .Include(b => b.Amenity)
                        .Include(b => b.User)
                        .Where(b => b.UserID == userId && b.BookingDate.Date >= DateTime.Now.Date)
                        .OrderBy(b => b.BookingDate);
            ViewBag.PaymentSuccessMessage = TempData["PaymentSuccessMessage"] as string;
            return View("Index", await bookings.ToListAsync());
        }

        public async Task<IActionResult> PastBookings()
        {
            var userId = User.FindFirst("UserID").Value;
            var pastBookings = _context.Booking
                .Include(b => b.Amenity)
                .Include(b => b.User)
                .Where(b => (b.UserID == userId && b.BookingDate.Date < DateTime.Now.Date))
                .OrderByDescending(b => b.BookingDate);
            return View("Index", await pastBookings.ToListAsync());
        }
    

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Amenity)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            var claims = User.Claims;
            var userID = User.FindFirst("UserID")?.Value;
            
            ViewData["Rates"] = getRates();
            ViewData["AmenityID"] = new SelectList(_context.Amenity, "AmenityID", "AmenityName");
            ViewData["UserID"] = userID;
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingID, UserID, AmenityID,BookingDate,NumberOfPeople,AmountPaid,PurchasedDate")] Booking booking)
        {
            ModelState.Remove("UserID");
            string userId = User.FindFirst("UserID")?.Value;
            string amenityId = booking.AmenityID;

            var user = await _context.User.FindAsync(userId);
            var amenity = await _context.Amenity.FindAsync(amenityId);

            booking.User = user;
            booking.UserID = userId;
            booking.Amenity = amenity;
            booking.AmountPaid = booking.NumberOfPeople * amenity.CostPerPerson;
            booking.PurchasedDate = DateTime.Now;

            if (booking.NumberOfPeople <= 0)
            {
                ViewData["AmenityID"] = new SelectList(_context.Amenity, "AmenityID", "AmenityName", booking.AmenityID);
                ViewData["Rates"] = getRates();
                ModelState.AddModelError(nameof(Booking.NumberOfPeople), 
                    "Number of people must be greater than zero.");
            }

            else if (!_amenityManagementService.IsBookingValid(booking))
            {
                ViewData["AmenityID"] = new SelectList(_context.Amenity, 
                    "AmenityID", "AmenityName", booking.AmenityID);
                ViewData["Rates"] = getRates();
                ModelState.AddModelError(string.Empty, 
                    "Sorry! Looks like we don't have enough space for this day.\nPlease choose another date.");
            }

            ModelState.Remove("BookingID");
            ModelState.Remove("User");
            ModelState.Remove("Amenity");

            if (ModelState.IsValid)
            {
                
                var bookingJson = JsonConvert.SerializeObject(booking);
                TempData["BookingData"] = bookingJson;
                return RedirectToAction("BookingPaymentForm", "Payment");
            }
            
            ViewData["AmenityID"] = new SelectList(_context.Amenity, "AmenityID", "AmenityName", booking.AmenityID);
            ViewData["Rates"] = getRates();
            return View(booking);
        }

        private Dictionary<string, decimal> getRates()
        {
            var rates = new Dictionary<string, decimal>();
            var amenityTable = _context.Amenity.ToList();
            amenityTable.ForEach(amenity => {
                rates[amenity.AmenityID] = amenity.CostPerPerson;
            });
            return rates;
        } 

        private Dictionary<string, string> getNames()
        {
            var names = new Dictionary<string, string>();
            var amenityTable = _context.Amenity.ToList();
            amenityTable.ForEach(amenity => {
                names[amenity.AmenityID] = amenity.AmenityName;
            });
            return names;
        }
    }
}
