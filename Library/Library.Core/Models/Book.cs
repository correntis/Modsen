using Microsoft.AspNetCore.Http;

namespace Library.Core.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime TakenAt { get; set; }
        public DateTime ReturnBy { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImagePath { get; set; }
        public ICollection<Author> Authors { get; set; } = [];
    }
}
