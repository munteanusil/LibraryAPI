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
                .ForMember(s => s.Author, des => des.MapFrom(s => s.Author != null ? new Author(s.Author) : null))
                .ForMember(s => s.Category, des => des.MapFrom(s => s.Category != null ? new Category { Id = s.Category.Id, Name = s.Category.Name } : null))
                .ReverseMap();
        }
    }
}
