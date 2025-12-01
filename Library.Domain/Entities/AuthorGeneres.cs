using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class AuthorGeneres
    {
        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        public int GenreId {  get; set; }

        public Genre? Genre { get; set; }
    }
}
