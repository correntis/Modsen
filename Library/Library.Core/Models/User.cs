using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Models
{
    public class User
    {
        public const int MIN_PASSWORD_LENGTH = 4;
        public const int MAX_PASSWORD_LENGTH = 32;
        public const int MAX_USERNAME_LENGTH = 16;

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<UserRole> Roles { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
