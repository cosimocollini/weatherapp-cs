// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using BlazorWeatherApp.Models;

namespace BlazorWeatherApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Favourites> Favourites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Favourites>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(100);
            });

            // Seed data
            modelBuilder.Entity<Favourites>().HasData(
                new Favourites { Id = 1, Name = "Milan" },
                new Favourites { Id = 2, Name = "Florence" },
                new Favourites { Id = 3, Name = "Rome" }
            );
        }
    }
}