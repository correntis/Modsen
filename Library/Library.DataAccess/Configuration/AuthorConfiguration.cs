using Library.Core.Models;
using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.Core.Constants;

namespace Library.DataAccess.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(b => b.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(AuthorConstants.MAX_NAME_LENGTH);

            builder.Property(a => a.Surname)
                .IsRequired()
                .HasMaxLength(AuthorConstants.MAX_SURNAME_LENGTH);

            builder.Property(a => a.Birthday)
                .IsRequired();

            builder.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(AuthorConstants.MAX_COUNTRY_LENGTH);
        }
    }
}
