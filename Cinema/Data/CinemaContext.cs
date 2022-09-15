using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cinema.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Cinema.Areas.Identity.Data;

namespace Cinema.Data
{
    public class CinemaContext : IdentityDbContext<CinemaUser>
    {
        public CinemaContext (DbContextOptions<CinemaContext> options)
            : base(options)
        {
        }

        public DbSet<Cinema.Models.Movie> Movie { get; set; } = default!;
        public DbSet<Cinema.Models.Client>? Client { get; set; }
        public DbSet<Cinema.Models.Screening>? Screening { get; set; }
        public DbSet<Cinema.Models.Reservation>? Reservation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Movie>().ToTable("Movie");
            builder.Entity<Client>().ToTable("Client");
            builder.Entity<Screening>().ToTable("Screening");
            builder.Entity<Reservation>().ToTable("Reservation");

            builder.Entity<Screening>()
                .HasOne<Movie>(p => p.Movie)
                .WithMany(p => p.Projections)
                .HasForeignKey(p => p.MovieId);//.OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>()
                .HasOne(p => p.Screening)
                .WithMany(p => p.Audience)
                .HasForeignKey(p => p.ScreeningId);//.OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>()
                .HasOne(p => p.User)
                .WithMany(p => p.Reservations)
                .HasForeignKey(p => p.ClientId);//.OnDelete(DeleteBehavior.NoAction);
        }
    }
}
