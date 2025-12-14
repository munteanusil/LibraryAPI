using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Common
{
    public class PaginatedList<T> where T : new()
    {

        public PaginatedList(List<T> items, int pageNumber, int pageTotals)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = pageTotals;

        }
        public List<T> Items { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public bool HasPrevious => PageNumber > 1;

        public bool HasNext => PageNumber < TotalPages;
    }
}
