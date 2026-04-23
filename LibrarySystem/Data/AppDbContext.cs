using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<BorrowRecord> BorrowRecords { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
}
