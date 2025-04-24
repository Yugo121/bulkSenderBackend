using Application.Models.Commands.MappingCommands;
using Application.Models.DTO_s;
using Application.Models.Queries.MappingQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bulk_sender_backend.Controllers
{
    public class MappingController : Controller
    {
        private readonly IMediator _mediator;
        public MappingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/mappings/names")]
        public async Task<IActionResult> GetMappingsNames()
        {
            var mappingNames = await _mediator.Send(new GetMappingsNamesQuery());
            return Ok(mappingNames);
        }
        [HttpGet]
        [Route("api/mappings/{name}")]
        public async Task<IActionResult> GetMapping(string name)
        {

            var mapping = await _mediator.Send(new GetMappingByNameQuery(name));
            return Ok(mapping);
        }
        [HttpPost]
        [Route("api/mappings")]
        public async Task<IActionResult> CreateMapping([FromBody] MappingDTO mapping)
        {
            if (mapping == null)
            {
                return BadRequest("Invalid mapping data.");
            }

            var createdMapping = await _mediator.Send(new AddMappingCommand(mapping));
            return Ok(createdMapping);
        }

    }
}
