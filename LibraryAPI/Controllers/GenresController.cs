using AutoMapper;
using Library.Application.DTOs;
using Library.Application.DTOs.Genres;
using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreRepository _repository;
    private readonly IMapper _mapper;

    public GenreController(IGenreRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateGenreDto genreDto, CancellationToken cancellationToken)
    {
        var genreToCreate = _mapper.Map<Genre>(genreDto);
        await _repository.CreateGenre(genreToCreate, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = genreToCreate.Id }, genreToCreate);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginatedDto dto, CancellationToken cancellationToken)
    {

        var paginatedGenres = await _repository.GetGenres(dto.Page, dto.PageSize, cancellationToken);
        var genresDto = _mapper.Map<List<GenreDto>>(paginatedGenres.Items);
        var result = new PaginatedList<GenreDto>(genresDto, dto.Page, dto.PageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var genre = await _repository.GetGenreById(id, cancellationToken);
        if (genre == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<GenreDto>(genre));
    }

    [HttpPut]
    public async Task<IActionResult> Put(GenreDto genreDto, CancellationToken cancellationToken)
    {
        await _repository.UpdateGenre(_mapper.Map<Genre>(genreDto), cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _repository.DeleteGenre(id, cancellationToken);
        return Ok();
    }
}