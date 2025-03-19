using Application.Models.Commands.BrandCommands;
using Application.Models.Commands.CategoryCommands;
using Application.Models.DTOs;
using Application.Models.Queries.BrandQueries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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
            GetBrandQuery query = new GetBrandQuery(id);
            BrandDTO brand = await _mediator.Send(query);

            return Ok(brand);
        }

        [HttpPost]
        [Route("api/category")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand command)
        {
            Guid brandId = await _mediator.Send(command);
            return Ok(brandId);
        }

        [HttpPut]
        [Route("api/category/edit/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] EditCategoryCommand command)
        {
            command.Category.Id = id;
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
