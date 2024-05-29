using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.Cards;

namespace RapidPay.Infrastructure.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(e => e.CardNumber)
                  .IsUnique();

            builder.Property(c => c.Balance)
            .HasPrecision(18, 2)
            .IsRequired();

            builder.HasData(
                Card.Create("91ddab95f49135173de8e8bf4cd905ac4bd0a77b58d004bf498cea145e6864ce", "Jhon", new DateTime(2025,1,1), "City Bank", 15000)
            );
        }
    }
}