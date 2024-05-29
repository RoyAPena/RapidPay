using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;
using RapidPay.Domain.User;

namespace RapidPay.Infrastructure.Data
{
    public sealed class RapidPayContext : DbContext
    {
        public RapidPayContext(DbContextOptions<RapidPayContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RapidPayContext).Assembly);
    }
}