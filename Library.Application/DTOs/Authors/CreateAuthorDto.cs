using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Authors
{
    public class CreateAuthorDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Nationality { get; set; }

        public string? Biography { get; set; }

        public string? Site { get; set; }
    }
}
