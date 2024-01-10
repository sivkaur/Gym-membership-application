using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitHub.Models;

namespace FitHub.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext (DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        public DbSet<FitHub.Models.Amenity> Amenity { get; set; } = default!;

        public DbSet<FitHub.Models.Sauna>? Sauna { get; set; }

        public DbSet<FitHub.Models.Spa>? Spa { get; set; }

        public DbSet<FitHub.Models.SwimmingPool>? SwimmingPool { get; set; }

        public DbSet<FitHub.Models.Booking>? Booking { get; set; }

        public DbSet<FitHub.Models.User>? User { get; set; }

        public DbSet<FitHub.Models.Membership>? Membership { get; set; }
        public DbSet<FitHub.Models.MembershipDetail>? MembershipDetail { get; set; }
    }
}
