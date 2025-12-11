using AutoMapper;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Books
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<CreateBookDto, Book>()
                .ReverseMap();
            CreateMap<Book, BookDto>()
               .ReverseMap();
        }
    }
}
