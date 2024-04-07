using Microsoft.EntityFrameworkCore;
using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Data;

public class BlogDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Content> Contents { get; set; }

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
