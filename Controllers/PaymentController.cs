using FitHub.Data;
using FitHub.Models;
using FitHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FitHub.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {

        private readonly GymDbContext _context;
        private readonly AmenityManagementService _amenityManagementService;

        public PaymentController(GymDbContext context, AmenityManagementService amenityManagementService)
        {
            _context = context;
            _amenityManagementService = amenityManagementService;
        }

        public IActionResult BookingPaymentForm()
        {
            var booking = JsonConvert.DeserializeObject<Booking>(TempData["BookingData"] as String);

            if (booking == null)
            {
                return NotFound();
            }

            return View("BookingPaymentForm", new PaymentMethod { Booking = booking });
        }

        [HttpPost]
        public async Task<IActionResult> ProcessBookingPayment(PaymentMethod paymentMethod)
        {

            string userId = paymentMethod.Booking.UserID;
            string amenityId = paymentMethod.Booking.AmenityID;

            var user = await _context.User.FindAsync(userId);
            var amenity = await _context.Amenity.FindAsync(amenityId);

            paymentMethod.Booking.User = user;
            paymentMethod.Booking.Amenity = amenity;

            // Always set the payment to success for this project
            if (ModelState.IsValid)
            {
                _context.Booking.Add(paymentMethod.Booking);
                _context.SaveChanges();
                _amenityManagementService.UpdateAmenityCapacity(paymentMethod.Booking);
                TempData["PaymentSuccessMessage"] = "Payment successful!";
                return RedirectToAction("Index", "Bookings");
            }
            var bookingJson = JsonConvert.SerializeObject(paymentMethod.Booking);
            TempData["BookingData"] = bookingJson;
            return View("BookingPaymentForm", paymentMethod);
        }
        public IActionResult MembershipPaymentForm()
        {
            var membership = JsonConvert.DeserializeObject<Membership>(TempData["MembershipData"] as String);

            if (membership == null)
            {
                return NotFound();
            }

            return View("MembershipPaymentForm", new PaymentMethod { Membership = membership });
        }

        [HttpPost]
        public async Task<IActionResult> ProcessMembershipPayment(PaymentMethod paymentMethod)
        {
            string userId = paymentMethod.Membership.UserID;
            string membTypeId = paymentMethod.Membership.MembershipTypeID;

            var user = await _context.User.FindAsync(userId);
            var membDetail = await _context.MembershipDetail.FindAsync(membTypeId);

            paymentMethod.Membership.User = user;
            paymentMethod.Membership.MD = membDetail;

            ModelState.Remove("Membership.MD");
            ModelState.Remove("Membership.User");
            ModelState.Remove("Membership.MembershipID");
            // Always set the payment to success for this project
            if (ModelState.IsValid)
            {
                _context.Membership.Add(paymentMethod.Membership);
                _context.SaveChanges();
                TempData["PaymentSuccessMessage"] = "Payment successful!";
                return RedirectToAction("Index", "Memberships");
            }

            var membershipJson = JsonConvert.SerializeObject(paymentMethod.Membership);
            TempData["MembershipData"] = membershipJson;
            return View("MembershipPaymentForm", paymentMethod);
        }
    }
}
