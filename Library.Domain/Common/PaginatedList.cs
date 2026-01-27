using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Common
{
    public class PaginatedList<T> where T : new()
    {
        public PaginatedList(List<T> items, int pageNumber, int totalPages)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = totalPages;
        }

        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public override string ToString()
        {
            var text = new StringBuilder($"PageNumber: {PageNumber} \n" +
                $"HasPreviour: {HasPrevious} \n" +
                $"HasNext: {HasNext} \n" +
                $"Total: {TotalPages}\n" +
                "---------------------\n");
            foreach (var item in Items)
            {
                text.Append(item?.ToString());
                text.Append("\n");
                text.Append("---------------------\n");
            }
            return text.ToString();
        }
    }
}
