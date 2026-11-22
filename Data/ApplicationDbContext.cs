using ContactManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Data;

// main db context, connects our models to the actual database
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // gives us access to the Contacts table
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>(entity =>
        {
            // phone number and email must be unique at the database level too
            entity.HasIndex(c => c.ContactNumber).IsUnique();
            entity.HasIndex(c => c.Email).IsUnique();

            // this filter hides soft-deleted contacts from all queries by default
            // use IgnoreQueryFilters() when you need to see deleted ones
            entity.HasQueryFilter(c => !c.IsDeleted);
        });
    }
}
