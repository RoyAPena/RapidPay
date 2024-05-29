using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.Transactions;

namespace RapidPay.Infrastructure.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Card)
                .WithMany(x => x.Transactions)
                .HasForeignKey(e => e.CardId);

            builder.Property(c => c.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

            builder.Property(c => c.Fee)
            .HasPrecision(18, 2)
            .IsRequired();
        }
    }
}