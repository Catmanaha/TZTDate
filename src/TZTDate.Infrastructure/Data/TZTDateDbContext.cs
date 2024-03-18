using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data;

public class TZTDateDbContext : IdentityDbContext<User, IdentityRole, string>
{
    DbSet<User> Users { get; set; }
    public TZTDateDbContext(DbContextOptions<TZTDateDbContext> options) : base(options) { }
}
