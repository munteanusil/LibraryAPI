using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Categories
{
    public class CategoryDto : CreateCategoryDto
    {
        public int Id { get; set; }

        public int Books { get; set; }

        public override string ToString()
        {
            return $"Category id: {Id} \n" +
                $"Category: {Name}";
        }
    }
}
