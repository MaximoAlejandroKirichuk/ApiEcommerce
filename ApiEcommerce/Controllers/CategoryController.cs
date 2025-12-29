using ApiEcommerce.Mapping;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetALLCategories()
        {
            var categories = _categoryRepository.GetAll();
            var categoriesDTO = new List<CategoryDTO>();
            foreach (var category in categories.Result.ToList())
            {
                var categoryDto = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreationDate = category.CreationDate
                };
                categoriesDTO.Add(categoryDto);
            }
            return Ok(categoriesDTO);
        }
        [HttpGet("Id/{id}", Name = "GetCategoryById")] 
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if(category == null) return NotFound($"Category not found {id}");
            var categoryDto = category.ToDto();
            return Ok(categoryDto);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult CreateCategory([FromBody]CreateCategoryDTO categoryCreateDto)
        {
            if(categoryCreateDto == null) return BadRequest(ModelState);
            if (_categoryRepository.CategoryExistsByName(categoryCreateDto.Name))
            {
                ModelState.AddModelError("Name", "Category already exists");
                return BadRequest(ModelState);
            }
            var category = categoryCreateDto.ToEntity();
            _categoryRepository.Create(category);
            return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category.ToDto());
        }
        [HttpPatch("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateCategory(int id,[FromBody]CreateCategoryDTO categoryCreateDto)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound("Category not exists");
            }
            if(categoryCreateDto == null) return BadRequest(ModelState);
            
            var category = categoryCreateDto.ToEntity();
            category.Id = id;
            if (!_categoryRepository.Update(category))
            {
                return StatusCode(500, "Category update failed");
            };
            return NoContent();
        }
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if(id == null) return BadRequest(ModelState);
            var category = await _categoryRepository.GetById(id);
            if (category ==  null)
            {
                return NotFound("Category not exists");
            }

            if (!_categoryRepository.Delete(category))
            {
                return StatusCode(500, "Category delete failed");
            };
            return NoContent();
        }
    }
}