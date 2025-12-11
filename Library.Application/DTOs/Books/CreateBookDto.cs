using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Books
{
    public class CreateBookDto
    {

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public string ISBN { get; set; }

        public int Stock { get; set; }
    }
}
