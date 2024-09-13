using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .HasMaxLength(Author.MAX_NAME_LENGTH);

            builder.Property(a => a.Surname)
                .IsRequired()
                .HasMaxLength(Author.MAX_SURNAME_LENGTH);

            builder.Property(a => a.Birthday)
                .IsRequired();

            builder.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(Author.MAX_COUNTRY_LENGTH);
        }
    }
}
