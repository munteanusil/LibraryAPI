using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Library.Application.DTOs;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookDto> _validator;
        private readonly IValidator<PaginatedDto> _paginatedValidator;

        public BookController(IBookRepository repository,
            IMapper mapper,
            IValidator<CreateBookDto> validator,
            IValidator<PaginatedDto> paginatedValidator)
        {
            _repository= repository;
            _mapper= mapper;
            _validator = validator;
            _paginatedValidator= paginatedValidator;
        }


        [HttpPost]
        public async Task<IActionResult> Post(CreateBookDto bookDto, CancellationToken cancellationToken)
        {
         
                await _repository.CreateBook(_mapper.Map<Book>(bookDto), cancellationToken);
                return Created();
            

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginatedDto dto, CancellationToken cancellationToken)
        {
          
               
            var books = await _repository.GetBooks(dto.Page, dto.PageSize);
            var booksDto = _mapper.Map<List<BookDto>>(books.Items);
            var result = new PaginatedList<BookDto>(booksDto, dto.Page, dto.PageSize);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(BookDto bookdto, CancellationToken cancellationToken)
        {
           
                await _repository.UpdateBook(_mapper.Map<Book>(bookdto),cancellationToken);
                return Ok();
            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            await _repository.DeleteBook(id,cancellationToken);
            return Ok();
        }

    }
}
