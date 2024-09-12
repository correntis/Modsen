using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Entities
{
    public class BookEntity
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime TakenAt { get; set; }
        public DateTime ReturnBy { get; set; }
        public string ImagePath { get; set; }

        public ICollection<AuthorEntity> Authors { get; set; } = [];
    }
}
