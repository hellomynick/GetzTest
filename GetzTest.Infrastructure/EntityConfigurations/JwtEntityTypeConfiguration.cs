using GetzTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetzTest.Infrastructure.EntityConfigurations;

public class JwtEntityTypeConfiguration : IEntityTypeConfiguration<Jwt>
{
    public void Configure(EntityTypeBuilder<Jwt> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AccountId).IsRequired();
        builder.Property(x => x.RefreshToken).HasMaxLength(256);
        builder.Property(x => x.ClientId);
        builder.Property(x => x.DeviceInformation).HasMaxLength(256);
        builder.Property(x => x.Expires);
    }
}
