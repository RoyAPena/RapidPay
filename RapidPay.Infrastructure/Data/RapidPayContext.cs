using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;

namespace RapidPay.Infrastructure.Data
{
    public sealed class RapidPayContext : IdentityDbContext<IdentityUser>
    {
        public RapidPayContext(DbContextOptions<RapidPayContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<IdentityUser>();

            var user = new IdentityUser
            {

                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = hasher.HashPassword(null, "Abc_12345"),
                Email = "admin@rapidpay.com",
                EmailConfirmed = true
            };

            // Only add the user if it doesn't already exist
            modelBuilder.Entity<IdentityUser>().HasData(user);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RapidPayContext).Assembly);
        }
    }
}