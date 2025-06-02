using Application.Models.Commands.BaselinkerCommands;
using Application.Models.DTOs;
using Application.Models.Queries.BaselinkerQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bulk_sender_backend.Controllers
{
    public class BaselinkerController : Controller
    {
        private readonly IMediator _mediator;
        public BaselinkerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("api/baselinker/products")]
        public async Task<IActionResult> AddProductToBaselinker([FromBody] ProductDTO product, CancellationToken cancellation)
        {
            //product.Category.BaselinkerId = 3013902;
            List<int> ids = await _mediator.Send(new AddProductToBaselinkerCommand(product));

            return Ok(ids);
        }

        [HttpGet]
        [Route("api/baselinker/brands")]
        public async Task<IActionResult> GetBrands(CancellationToken cancellation)
        {
            string brands = await _mediator.Send(new GetBaselinkerBrandsQuery());
            return Ok(brands);
        }

        [HttpGet]
        [Route("api/baselinker/categories")]
        public async Task<IActionResult> GetCategories(CancellationToken cancellation)
        {
            string categories = await _mediator.Send(new GetBaselinkerCategoriesQuery());
            return Ok(categories);
        }
    }
}
