using Microsoft.EntityFrameworkCore;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.Core.Models;

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
                .HasMaxLength(UserRole.MAX_NAME_LENGTH);
        }
    }
}
