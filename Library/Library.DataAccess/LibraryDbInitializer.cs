using Library.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess
{
    public class LibraryDbInitializer
    {
        public static async Task InitializeAsync(LibraryDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (context.Authors.Any() || context.Books.Any() || context.Books.Any())
            {
                return;
            }   

            await SeedAuthorsAsync(context);
            await SeedBooksAsync(context);
            await SeedUserRolesAsync(context);
            await SeedUsersAsync(context);
        }

        private static async Task SeedAuthorsAsync(LibraryDbContext context)
        {
            await context.Authors.AddRangeAsync(GetAuthors());
            await context.SaveChangesAsync();
        }

        private static async Task SeedBooksAsync(LibraryDbContext context)
        {
            await context.Books.AddRangeAsync(await GetBooksAsync(context));
            await context.SaveChangesAsync();
        }

        private static async Task SeedUserRolesAsync(LibraryDbContext context)
        {
            await context.UserRoles.AddRangeAsync(GetUserRoles());
            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(LibraryDbContext context)
        {
            await context.Users.AddRangeAsync(await GetUsersAsync(context));
            await context.SaveChangesAsync();
        }

        private static List<AuthorEntity> GetAuthors()
        {
            return
            [
                new() {
                    Name = "Joanne",
                    Surname = "Rowling",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "England"
                },
                new() {
                    Name = "George",
                    Surname= "Orwell",
                    Birthday = DateTime.SpecifyKind(new DateTime(1903, 6, 25), DateTimeKind.Utc),
                    Country = "England"
                },
                new() {
                    Name = "William",
                    Surname = "Shakespeare",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "England"
                },
                new() {
                    Name = "Stephen",
                    Surname = "King",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "USA"
                },
                new() {
                    Name = "J.R.R.",
                    Surname = "Tolkien",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "England"
                },
                new() {
                    Name = "Agatha",
                    Surname = "Christie",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "England"
                },
                new() {
                    Name = "Arthur",
                    Surname = "Conan Doyle",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "Scotland"
                },
                new() {
                    Name = "Anton",
                    Surname = "Chekhov",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "Russia"
                },
                new() {
                    Name = "Fyodor",
                    Surname = "Dostoevsky",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "Russia"
                },
                new() {
                    Name = "Lev",
                    Surname = "Tolstoy",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "Russia"
                },
                new() {
                    Name = "Mikhail",
                    Surname = "Bulgakov",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "Russia"
                },
                new() {
                    Name = "Ernest",
                    Surname = "Hemingway",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "USA"
                },
                new() {
                    Name = "Mark",
                    Surname = "Twain",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "USA"
                },
                new() {
                    Name = "Charles",
                    Surname = "Dickens",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "England"
                },
                new() {
                    Name = "Oscar",
                    Surname = "Wilde",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "Ireland"
                },
                new() {
                    Name = "Edgar Allan",
                    Surname = "Poe",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "USA"
                },
                new() {
                    Name = "Herman",
                    Surname = "Melville",
                    Birthday = DateTime.SpecifyKind(new DateTime(1965, 7, 31), DateTimeKind.Utc),
                    Country = "USA"
                },
            ];

        }

        private async static Task<List<BookEntity>> GetBooksAsync(LibraryDbContext context)
        {
            var authors = await context.Authors.ToListAsync();

            return
            [
                new()
                {
                    Name = "1984",
                    Genre = "Dystopian",
                    Description = "A dystopian social science fiction novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780451524935",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Orwell").ToList()
                },
                new()
                {
                    Name = "Animal Farm",
                    Genre = "Political satire",
                    Description = "An allegorical novella",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780451526342",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Orwell").ToList()
                },
                new()
                {
                    Name = "Harry Potter and the Philosopher's Stone",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780747532743",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Rowling").ToList()
                },
                new() 
                {
                    Name = "Harry Potter and the Chamber of Secrets",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780747538493",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Rowling").ToList()
                },
                new()
                {
                    Name = "Harry Potter and the Prisoner of Azkaban",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780747546290",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Rowling").ToList()
                },
                new()
                {
                    Name = "Harry Potter and the Goblet of Fire",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780747546245",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Rowling").ToList()
                },
                new()
                {
                    Name = "Harry Potter and the Order of the Phoenix",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780747546245",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Rowling").ToList()
                },
                new()
                {
                    Name = "Harry Potter and the Half-Blood Prince",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780747546245",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Rowling").ToList()
                },
                new()
                {
                    Name = "Harry Potter and the Deathly Hallows",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780747546245",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Rowling").ToList()
                },
                new()
                {
                    Name = "The Lord of the Rings",
                    Genre = "High fantasy",
                    Description = "An epic high-fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780618640157",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Tolkien").ToList()
                },
                new()
                {
                    Name = "The Hobbit",
                    Genre = "Fantasy",
                    Description = "A fantasy novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780618260300",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Tolkien").ToList()
                },
                new()
                {
                    Name = "The Silmarillion",
                    Genre = "High fantasy",
                    Description = "A collection of mythopoeic works",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780618391110",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Tolkien").ToList()
                },
                new()
                {
                    Name = "The Great Gatsby",
                    Genre = "Tragedy",
                    Description = "A novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780743273565",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Fitzgerald").ToList()
                },
                new()
                {
                    Name = "The Catcher in the Rye",
                    Genre = "Realistic fiction",
                    Description = "A novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780316769488",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Salinger").ToList()
                },
                new()
                {
                    Name = "To Kill a Mockingbird",
                    Genre = "Southern Gothic",
                    Description = "A novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780060935467",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Lee").ToList()
                },
                new()
                {
                    Name = "Pride and Prejudice",
                    Genre = "Romantic",
                    Description = "A novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780141439518",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Austen").ToList()
                },
                new()
                {
                    Name = "Jane Eyre",
                    Genre = "Gothic",
                    Description = "A novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780142437209",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Bronte").ToList()
                },
                new()
                {
                    Name = "Wuthering Heights",
                    Genre = "Gothic",
                    Description = "A novel",
                    ImagePath = "default_image.jpg",
                    ISBN = "9780141439556",
                    ReturnBy = DateTime.MinValue,
                    TakenAt = DateTime.MinValue,
                    Authors = authors.Where(a => a.Surname == "Bronte").ToList()
                }
            ];
        }

        private static List<UserRoleEntity> GetUserRoles()
        {
            return
            [
                new()
                {
                    Name = "Admin",
                },
                new() 
                {
                    Name = "User"
                }
            ];
        }

        private async static Task<List<UserEntity>> GetUsersAsync(LibraryDbContext context)
        {
            var roles = await context.UserRoles.ToListAsync();

            return
            [
                new()
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEKGiyp21DNXhyuDAWFbIMoVyNXlezgur6EjYTYT4sItL6tUjNa6EH5GHIpEC/laQwg==", // admin
                    Roles = [.. roles],
                },
                new()
                {
                    UserName = "user",
                    Email = "user@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAENfGr+uwdYcmm5qc5Oi75KNHFhKkcZkn184lFdSCYn12TyV/nWotxOz3shjVjK9BaA==", // user
                    Roles = roles.Where(r => r.Name == "User").ToList(),
                }
            ];
        }
    }
}
