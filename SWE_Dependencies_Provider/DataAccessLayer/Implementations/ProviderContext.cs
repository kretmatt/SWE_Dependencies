using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementations;

public class ProviderContext:DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<FinancialProduct> FinancialProducts { get; set; }

    public ProviderContext(DbContextOptions<ProviderContext> options):base(options)
    {
        // Just to ensure that the DB is fresh every time the service starts, to quickly implement changes in the entities / DTOs tables
        //this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
    }
}