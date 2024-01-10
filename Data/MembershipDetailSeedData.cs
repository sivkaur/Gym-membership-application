using System;
using System.Linq;
using FitHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitHub.Data
{
    public static class MembershipDetailSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var gymDbContext = new GymDbContext(serviceProvider.GetRequiredService<DbContextOptions<GymDbContext>>());

            // Check if there are any existing MembershipDetails
            if (gymDbContext.MembershipDetail.Any())
            {
                return; // DB has been seeded
            }

            // Add sample data
            var membershipDetails = new MembershipDetail[]
            {
                new MembershipDetail
                {
                    MembershipTypeID = "1",
                    MembershipTypeName = "Standard",
                    DurationMonths = 6,
                    Cost = 400,
                    Description = "💫 Standard Membership! Unleash 6 months of boundless fitness opportunities at just $400.00. Embrace a transformative journey with access to top-notch facilities, personalized training sessions, exclusive fitness classes, and unrivaled wellness amenities. Elevate your fitness aspirations and embrace a healthier lifestyle with our six-month membership package tailored to bring out the best in you!"
                },
                new MembershipDetail
                {
                    MembershipTypeID = "2",
                    MembershipTypeName = "Premium",
                    DurationMonths = 12,
                    Cost = 750,
                    Description = "✨ Premium Membership! Elevate your fitness journey with a year-long pass to a world of wellness. Unleash the power of this membership, granting you a year of unparalleled access to our top-tier facilities and services. With an investment of $750.00, immerse yourself in a transformative fitness experience, where every visit promises an enriching blend of state-of-the-art equipment, personalized training sessions, rejuvenating amenities, and a vibrant community dedicated to achieving peak health and vitality. Join us on this extraordinary fitness expedition and redefine your limits with the Premium Membership!"
                },
                new MembershipDetail
                {
                    MembershipTypeID = "3",
                    MembershipTypeName = "Student",
                    DurationMonths = 1,
                    Cost = 50,
                    Description = "🎓 Student Membership! Ideal for the on-the-go scholar! This membership offers full access to our top-notch facilities for a duration of 1 month at an unbeatable price of $50.00. Embrace your fitness journey without breaking the bank, ensuring a balance between academic pursuits and a healthy lifestyle. Elevate your energy, stay fit, and thrive academically with our exclusive student membership!"
                }
            };

            // Add the sample data to the context
            gymDbContext.MembershipDetail.AddRange(membershipDetails);
            gymDbContext.SaveChanges();
        }
    }
}
