using Library.Application.DTOs.Authors;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Books
{
    public class BookDto : CreateBookDto
    {
        public int Id { get; set; }

        public AuthorDto? Author { get; set; }

        public override string ToString()
        {
            return $"BookId: {Id} \n" +
                $"BookTitle: {Title}\n" +
                $"ISBN: {ISBN}";
        }
    }


}
