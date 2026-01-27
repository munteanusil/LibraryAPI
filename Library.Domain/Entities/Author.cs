using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Author
    {
        public Author()
        {
            
        }
        public Author(Author author)
        {
            Id = author.Id;
            FirstName = author.FirstName;
            LastName = author.LastName;
            Nationality = author.Nationality;
            Site = author.Site;
        }

        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty; 

        public string LastName { get; set; } = string.Empty;

        public string? Nationality { get; set; }

        public string? Biography { get; set; }
 
        public string? Site { get; set; }

        public ICollection<AuthorGenres>? AuthorGenres { get; set; }

        public ICollection<Book>? Books { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
