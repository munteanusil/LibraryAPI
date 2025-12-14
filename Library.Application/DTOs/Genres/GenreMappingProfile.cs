using AutoMapper;
using Library.Application.DTOs.Categories;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Genres
{
    public class GenreMappingProfile : Profile
    { 
        public GenreMappingProfile()
        {

            CreateMap<CreateGenreDto, Genre>()
                  .ReverseMap();
            CreateMap<Genre, GenreDto>()
                .ReverseMap();
        }
    
    }
}
