using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data;

public class TZTDateDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public TZTDateDbContext(DbContextOptions<TZTDateDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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
