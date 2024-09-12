using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Library.Core.Models
{
    public class Book
    {
        public const int MAX_ISBN_LENGTH = 13;
        public const int MAX_NAME_LENGTH = 100;
        public const int MAX_GENRE_LENGTH = 50;
        public const int MAX_DESCRIPTION_LENGTH = 500;

        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime TakenAt { get; set; }
        public DateTime ReturnBy { get; set; }
        public string ImagePath { get; set; }
        public ICollection<Author> Authors { get; set; } = [];
    }
}
