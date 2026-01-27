using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Library.Application.DTOs;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookDto bookDto, CancellationToken cancellationToken)
        {
            var bookToCreate = _mapper.Map<Book>(bookDto);
            await _repository.CreateBook(bookToCreate, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = bookToCreate.Id }, _mapper.Map<BookDto>(bookToCreate));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginatedDto dto, CancellationToken cancellationToken)
        {

            var paginatedBooks = await _repository.GetBooks(dto.Page, dto.PageSize, cancellationToken);
            var bookDtos = _mapper.Map<List<BookDto>>(paginatedBooks.Items);
            var result = new PaginatedList<BookDto>(bookDtos, dto.Page, paginatedBooks.TotalPages);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var book = await _repository.GetBookById(id, cancellationToken);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<BookDto>(book));
        }

        [HttpPut]
        public async Task<IActionResult> Put(BookDto bookDto, CancellationToken cancellationToken)
        {
            await _repository.UpdateBook(_mapper.Map<Book>(bookDto), cancellationToken);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _repository.DeleteBook(id, cancellationToken);
            return Ok();
        }
    }

}
