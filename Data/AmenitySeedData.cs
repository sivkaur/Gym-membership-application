using FitHub.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FitHub.Data
{
    public class AmenitySeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var gymDbContext = new GymDbContext(serviceProvider.GetRequiredService<DbContextOptions<GymDbContext>>()))
            {
                if (gymDbContext.Amenity.Any())
                {
                    return;
                }

                gymDbContext.Amenity.AddRange(
                    new Amenity
                    {
                        AmenityID = "1",
                        AmenityName = "Swimming Pool",
                        MaxCapacityPerDay = 30,
                        CostPerPerson = 15
                    },
                    new Amenity
                    {
                        AmenityID = "2",
                        AmenityName = "Sauna",
                        MaxCapacityPerDay = 10,
                        CostPerPerson = 25
                    },
                    new Amenity
                    {
                        AmenityID = "3",
                        AmenityName = "Spa",
                        MaxCapacityPerDay = 20,
                        CostPerPerson = 50
                    }
                );

                gymDbContext.SaveChanges();
            }
        }
    }
}
