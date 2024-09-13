using Library.Core.Models;

namespace Library.API.Contracts
{
    public class AuthorContract
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
    }
}
