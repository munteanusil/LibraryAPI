using AutoMapper;
using FluentValidation;
using Library.Application.DTOs;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            var validationResult = await _validator.ValidateAsync(bookDto, cancellationToken);
            if (validationResult.IsValid)
            {
                await _repository.CreateBook(_mapper.Map<Book>(bookDto), cancellationToken);
                return Created();
            }
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginatedDto dto, CancellationToken cancellationToken)
        {
            var validationResult = await _paginatedValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select( e =>e.ErrorMessage));
            }
            var books = await _repository.GetBooks(dto.Page, dto.PageSize);
            var booksDto = _mapper.Map<List<BookDto>>(books.Items);
            var result = new PaginetedList<BookDto>(booksDto, dto.Page, dto.PageSize);
            return Ok(result);
        }

    }
}
