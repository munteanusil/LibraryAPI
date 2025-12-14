using AutoMapper;
using FluentValidation;
using Library.Application.DTOs;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Categories;
using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
      
        public CategoryController(ICategoryRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var categortyToCreate = _mapper.Map<Category>(categoryDto);
            await _repository.CreateCategory(categortyToCreate, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = categortyToCreate.Id, categortyToCreate });
         
          
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginatedDto dto, CancellationToken cancellationToken)
        {
            var categories = await _repository.GetCategorys(dto.Page, dto.PageSize, cancellationToken);
            var categoriesDtos = _mapper.Map<List<CategoryDto>>(categories.Items);
            var result = new PaginatedList<CategoryDto>(categoriesDtos, dto.Page, dto.PageSize);
            return Ok(result);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var category = await _repository.GetCategoryById(id, cancellationToken);
            if(category == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CreateCategoryDto>(category));
        }

        [HttpPut]
        public async Task<IActionResult> Put(CategoryDto categoryDto,CancellationToken cancellationToken)
        {
          
                await _repository.UpdateCategory(_mapper.Map<Category>(categoryDto), cancellationToken);
                return Ok();   
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _repository.DeleteCategory(id, cancellationToken);
            return Ok();
        }
    }
}
