using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Models
{
    public class UserRole
    {
        public const int MAX_NAME_LENGTH = 50;

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
