using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Authors
{
    public class AuthorDto : CreateAuthorDto
    {
        public int Id { get; set; }

        public override string ToString()
        {
            var age = (int)Math.Ceiling((DateTime.UtcNow - BirthDate).TotalDays / 356);
            return $"Author Name: {FirstName} {LastName} \n " +
                $"Author Age: {age} \n" +
                $"Author Site: {Site} \n";
        }
    }
}
