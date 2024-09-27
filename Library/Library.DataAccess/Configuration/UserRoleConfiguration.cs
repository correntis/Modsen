using Microsoft.EntityFrameworkCore;
using Library.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.Core.Constants;

namespace Library.DataAccess.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.HasKey(ur => ur.Id);
            builder.Property(ur => ur.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(ur => ur.Name)
                .HasMaxLength(UserRoleConstants.MAX_NAME_LENGTH);
        }
    }
}
