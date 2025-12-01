using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Nationality { get; set; }

        public string? Biography { get; set; }

        public string Site { get; set; }
        public ICollection<AuthorGeneres> AuthorGeneres { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
