using Library.Core.Models;
using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.Core.Constants;

namespace Library.DataAccess.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(BookConstants.MAX_ISBN_LENGTH);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(BookConstants.MAX_NAME_LENGTH);

            builder.Property(b => b.Description)
                .HasMaxLength(BookConstants.MAX_DESCRIPTION_LENGTH);

            builder.Property(b => b.Genre)
                .IsRequired()
                .HasMaxLength(BookConstants.MAX_GENRE_LENGTH);

            builder.Property(b => b.TakenAt)
                .IsRequired();

            builder.Property(b => b.ReturnBy)
                .IsRequired();

            builder.Property(b => b.ImagePath)
                .IsRequired();

            builder.HasMany(b => b.Authors)
                .WithMany()
                .UsingEntity(j => j.ToTable("BooksAuthors"));
        }
    }
}
