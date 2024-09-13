using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .HasMaxLength(Book.MAX_ISBN_LENGTH);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(Book.MAX_NAME_LENGTH);

            builder.Property(b => b.Description)
                .HasMaxLength(Book.MAX_DESCRIPTION_LENGTH);

            builder.Property(b => b.Genre)
                .IsRequired()
                .HasMaxLength(Book.MAX_GENRE_LENGTH);

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
