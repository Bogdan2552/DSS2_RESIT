using Microsoft.EntityFrameworkCore;
using MovieWatchlist.Api.Models;

namespace MovieWatchlist.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();
    public DbSet<CommunityMovie> CommunityMovies => Set<CommunityMovie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.HasIndex(user => user.NormalizedEmail).IsUnique();
            entity.Property(user => user.Email).HasMaxLength(256).IsRequired();
            entity.Property(user => user.NormalizedEmail).HasMaxLength(256).IsRequired();
            entity.Property(user => user.DisplayName).HasMaxLength(40).IsRequired();
            entity.Property(user => user.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<WatchlistItem>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Title).HasMaxLength(120).IsRequired();
            entity.Property(item => item.Director).HasMaxLength(80);
            entity.Property(item => item.Genre).HasMaxLength(40);
            entity.Property(item => item.Status).HasMaxLength(20).IsRequired();
            entity.Property(item => item.Notes).HasMaxLength(1000);
            entity.HasOne(item => item.User)
                .WithMany(user => user.WatchlistItems)
                .HasForeignKey(item => item.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityMovie>(entity =>
        {
            entity.HasKey(movie => movie.Id);
            entity.Property(movie => movie.Title).HasMaxLength(120).IsRequired();
            entity.Property(movie => movie.Director).HasMaxLength(80);
            entity.Property(movie => movie.Genre).HasMaxLength(40);
            entity.Property(movie => movie.Description).HasMaxLength(1500);
            entity.Property(movie => movie.CreatedByDisplayName).HasMaxLength(40).IsRequired();
            entity.HasOne(movie => movie.CreatedByUser)
                .WithMany(user => user.CommunityMovies)
                .HasForeignKey(movie => movie.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
