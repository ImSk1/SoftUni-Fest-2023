namespace SofwareFest.Infrastructure
{

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using SoftwareFest.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Business> Businesses { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Transaction>()
                .Property(t => t.Date)
                .HasDefaultValue(DateTime.UtcNow);

            builder.Entity<Transaction>()
                .HasOne(t => t.Client)
                .WithMany(c => c.Transactions)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Product>()
                .Property(x => x.EthPrice).HasPrecision(18, 10);
        }
    }
}
