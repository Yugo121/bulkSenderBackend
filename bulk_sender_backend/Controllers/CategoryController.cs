using Application.Models.Commands.CategoryCommands;
using Application.Models.DTOs;
using Application.Models.Queries.CategoryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bulk_sender_backend.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/category/{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            GetCategoryQuery query = new GetCategoryQuery(id);
            CategoryDTO categoryId = await _mediator.Send(query);

            return Ok(categoryId);
        }

        [HttpGet]
        [Route("api/categories")]
        public async Task<IActionResult> GetAllCategories() 
        {
            GetAllCategoriesQuery query = new GetAllCategoriesQuery();
            List<CategoryDTO> categories = await _mediator.Send(query);

            return Ok(categories);
        }

        [HttpGet]
        [Route("api/categories/names")]
        public async Task<IActionResult> GetCategoriesNames()
        {
            GetCategoriesNamesQuery query = new GetCategoriesNamesQuery();
            List<string> categories = await _mediator.Send(query);
            return Ok(categories);
        }

        [HttpPost]
        [Route("api/category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        {
            AddCategoryCommand command = new AddCategoryCommand(category);
            Guid categoryId = await _mediator.Send(command);
            return Ok(categoryId);
        }

        [HttpPut]
        [Route("api/category/edit/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody]  CategoryDTO category)
        {
            if (id != category.Id)
                return BadRequest("Category ID mismatch");

            EditCategoryCommand command = new EditCategoryCommand(category);
            Guid categoryId = await _mediator.Send(command);

            return Ok(categoryId);
        }

        [HttpDelete]
        [Route("api/category/delete/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            DeleteCategoryCommand command = new DeleteCategoryCommand(id);
            Guid categoryId = await _mediator.Send(command);

            return Ok(categoryId);
        }
    }
}
