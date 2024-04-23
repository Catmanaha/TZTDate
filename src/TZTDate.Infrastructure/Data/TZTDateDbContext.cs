using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateLogEntry.Models;
using TZTDate.Core.Data.DateToken.Models;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data;

public class TZTDateDbContext : DbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public TZTDateDbContext(DbContextOptions<TZTDateDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        builder.Entity<UserFollow>()
            .HasKey(uf => new { uf.FollowerId, uf.FollowedId });

        builder.Entity<UserFollow>()
            .HasOne(uf => uf.Follower)
            .WithMany(u => u.Followers)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserFollow>()
            .HasOne(uf => uf.Followed)
            .WithMany(u => u.Followed)
            .HasForeignKey(uf => uf.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<User>()
            .Property(e => e.ProfilePicPaths)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries
            )
        );
    }
}
