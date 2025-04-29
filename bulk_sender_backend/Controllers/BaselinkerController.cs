using Application.Models.Commands.BaselinkerCommands;
using Application.Models.DTOs;
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
            List<int> ids = await _mediator.Send(new AddProductToBaselinkerCommand(product));

            return Ok(ids);
        }
    }
}
