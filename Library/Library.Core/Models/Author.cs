namespace Library.Core.Models
{
    public class Author
    {
        public const int MAX_NAME_LENGTH = 50;
        public const int MAX_SURNAME_LENGTH = 50;
        public const int MAX_COUNTRY_LENGTH = 50;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }

        public ICollection<Book> Books { get; set; } = [];

    }
}
