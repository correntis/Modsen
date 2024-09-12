namespace Library.DataAccess.Entities
{
    public class AuthorEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }

        public ICollection<BookEntity> Books { get; set; } = [];
    }
}
