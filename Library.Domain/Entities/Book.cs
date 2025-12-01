using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public string ISBN { get; set; }

        public int Stock {  get; set; }
        public Category? Category { get; set; }

        public int AuthorId { get; set; }

        public Author? Author { get; set; }
    }
}
