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
    public class MembershipsController : Controller
    {
        private readonly GymDbContext _context;
        private readonly MembershipManagementService _membMgmtService;

        public MembershipsController(GymDbContext context, MembershipManagementService membMgmtService)
        {
            _context = context;
            _membMgmtService = membMgmtService;
        }

        // GET: Memberships
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserID").Value;
            var memberships = _context.Membership.Include(m => m.MD).Include(m => m.User)
                        .Where(m => (m.UserID == userId && m.EndDate >= DateTime.Now) )
                        .OrderBy(m => m.StartDate);
            ViewData["MembershipTypeID"] = new SelectList(_context.Set<MembershipDetail>(), "MembershipTypeID", "MembershipTypeName");
            ViewData["UserID"] = userId;
            ViewData["Details"] = getDetails();
            ViewBag.PaymentSuccessMessage = TempData["PaymentSuccessMessage"] as string;
            return View("Index", await memberships.ToListAsync());
            
        }

        // GET: Memberships/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Membership == null)
            {
                return NotFound();
            }

            var membership = await _context.Membership
                .Include(m => m.MD)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MembershipID == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: Memberships/Create
        public IActionResult Create()
        {
            var claims = User.Claims;
            var userID = User.FindFirst("UserID")?.Value;

            ViewData["MembershipTypeID"] = new SelectList(_context.Set<MembershipDetail>(), "MembershipTypeID", "MembershipTypeName");
            ViewData["UserID"] = userID;
            ViewData["Details"] = getDetails();
            return View();
        }

        // POST: Memberships/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipID,UserID,MembershipType,StartDate,EndDate,AmountPaid,MembershipTypeID")] Membership membership)
        {
            var claims = User.Claims;
            var userID = User.FindFirst("UserID")?.Value;
            
            string membTypeId = membership.MembershipTypeID;

            var user = await _context.User.FindAsync(userID);
            membership.UserID = user.UserID;
            var membDetail = await _context.MembershipDetail.FindAsync(membTypeId);
           
            
            membership.User = user;
            membership.MD = membDetail;

            membership.AmountPaid = membDetail.Cost;
            membership.EndDate = _membMgmtService.GetMembershipEndDate(membTypeId, membership.StartDate);

            ModelState.Remove("MD");
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("MembershipID");

            var memberships = _context.Membership.Include(m => m.MD).Include(m => m.User)
                        .Where(m => (m.UserID == userID && m.EndDate >= DateTime.Now))
                        .OrderBy(m => m.StartDate);
            foreach(var item in memberships)
            {
                if (item.EndDate > membership.StartDate)
                {
                    ModelState.AddModelError(string.Empty, "You cannot select this start date as you have an active membership till " + item.EndDate.ToString("MM/dd/yyyy"));
                    ViewData["MembershipTypeID"] = new SelectList(_context.Set<MembershipDetail>(), "MembershipTypeID", "MembershipTypeName", membership.MembershipTypeID);
                    ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", membership.UserID);
                    return View(membership);
                }
            }

            if (ModelState.IsValid)
            {
                var membershipJson = JsonConvert.SerializeObject(membership);
                TempData["MembershipData"] = membershipJson;
                return RedirectToAction("MembershipPaymentForm", "Payment");
            }
            ViewData["MembershipTypeID"] = new SelectList(_context.Set<MembershipDetail>(), "MembershipTypeID", "MembershipTypeName", membership.MembershipTypeID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", membership.UserID);
            return View(membership);
        }

        public IActionResult GetMembershipDescription(string membershipTypeId)
        {
            
            string description = _membMgmtService.GetMembershipDetails(membershipTypeId); 

            return Content(description);
        }

        private Dictionary<string, string> getDetails()
        {
            var details = new Dictionary<string, string>();
            var membDetails = _context.MembershipDetail.ToList();
            membDetails.ForEach(membDetails =>
            {
                details[membDetails.MembershipTypeID] = membDetails.Description;
            });
            return details;
        }

        private bool MembershipExists(string id)
        {
            return (_context.Membership?.Any(e => e.MembershipID == id)).GetValueOrDefault();
        }
    }
}
