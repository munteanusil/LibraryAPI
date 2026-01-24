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
            return $"📖 *Detalii Carte*\n\n" +
            $"🆔 **ID**: {Id}\n" +
            $"📚 **Titlu**: {Title}\n" +
            $"✍️ **Autor**: {Author}\n" + 
            $"🔢 **ISBN**: {ISBN}";
        }

    }
    
    
}
