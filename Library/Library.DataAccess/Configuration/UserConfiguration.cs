using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Email)
                .IsRequired();

            builder.Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.HasMany(u => u.Books)
                .WithMany()
                .UsingEntity(j => j.ToTable("UsersBooks"));

            builder.HasMany(u => u.Roles)
                .WithMany()
                .UsingEntity(j => j.ToTable("UsersRoles"));
        }
    }
}
