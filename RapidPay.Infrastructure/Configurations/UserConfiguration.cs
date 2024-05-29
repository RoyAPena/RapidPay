using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.User;

namespace RapidPay.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Username)
                  .IsUnique();

            builder.HasData(
                User.Create("Admin", "M1AG45Rf2YDWDmRhTObzJQ==")
            );
        }
    }
}