using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateChat.Entities;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data;

public class TZTDateDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Message> Message { get; set; }
    public DbSet<PrivateChat> PrivateChats { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public TZTDateDbContext(DbContextOptions<TZTDateDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .Property(e => e.ProfilePicPaths)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries
            )
        );
    }
}
