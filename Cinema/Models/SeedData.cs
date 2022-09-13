
using Cinema.Areas.Identity.Data;
using Cinema.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<CinemaUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            CinemaUser user = await UserManager.FindByEmailAsync("admin@cineworld.com");
            if (user == null)
            {
                var User = new CinemaUser();
                User.Email = "admin@cineworld.com";
                User.UserName = "admin@cineworld.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            // creating Client role     
            var x = await RoleManager.RoleExistsAsync("Client");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Client";
                await RoleManager.CreateAsync(role);
            }
        }

        public static void Initialze(IServiceProvider serviceProvider)
        {
            using(var context = new CinemaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CinemaContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Movie.Any() || context.User.Any() || context.Screening.Any() || context.Reservation.Any())
                {
                    return;
                }

                context.Movie.AddRange(
                    new Movie { Title = "Top Gun: Maverick", Genre = "Action", Duration = 131, ReleaseDate = DateTime.Parse("2022-05-27"), Rating = 8.6m },
                    new Movie { Title = "Elvis", Genre = "Drama", Duration = 169, ReleaseDate = DateTime.Parse("2022-06-23"), Rating = 7.8m },
                    new Movie { Title = "Minions: The Rise of Gru", Genre = "Animated", Duration = 90, ReleaseDate = DateTime.Parse("2022-06-30"), Rating = 6.9m },
                    new Movie { Title = "Thor: Love and Thunder", Genre = "Action/Fantasy", Duration = 119, ReleaseDate = DateTime.Parse("2022-07-07"), Rating = 6.8m },
                    new Movie { Title = "Jurassic World: Dominion", Genre = "Action/Sci-Fi", Duration = 147, ReleaseDate = DateTime.Parse("2022-06-09"), Rating = 5.7m }
                );
                context.SaveChanges();

                context.User.AddRange(
                    new User { FirstName = "Emilija", LastName = "Chona", Age = 21},
                    new User { FirstName = "Natalija", LastName = "Chona", Age = 18 },
                    new User { FirstName = "Aleksandar", LastName = "Rizev", Age = 21 }
                );
                context.SaveChanges();

                context.Screening.AddRange(
                    new Screening { MovieId = 1, Date = DateTime.Parse("2022-05-29"), Time = DateTime.Parse("19:00"),  Cinema = "Hall 2", AvailablePlaces = 80, Price = 250, Type = "2D" },
                    new Screening { MovieId = 1, Date = DateTime.Parse("2022-05-29"), Time = DateTime.Parse("22:00"), Cinema = "Hall 4", AvailablePlaces = 80, Price = 250, Type = "2D" },
                    new Screening { MovieId = 2, Date = DateTime.Parse("2022-07-30"), Time = DateTime.Parse("21:00"), Cinema = "Hall 1", AvailablePlaces = 100, Price = 250, Type = "2D" },
                    new Screening { MovieId = 3, Date = DateTime.Parse("2022-08-03"), Time = DateTime.Parse("17:00"), Cinema = "Hall 2", AvailablePlaces = 80, Price = 350, Type = "3D" }
                );
                context.SaveChanges();

                context.Reservation.AddRange(
                    new Reservation { ScreeningId = 1, UserId = 1, Seat = 12},
                    //new Reservation { ScreeningId = 2, UserId = 3, Seat = 34 },
                    new Reservation { ScreeningId = 4, UserId = 2, Seat = 25 }
                );
                context.SaveChanges();
            }
        }
    }
}
