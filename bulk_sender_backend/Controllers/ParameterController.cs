using Application.Models.Commands.ParameterCommands;
using Application.Models.DTOs;
using Application.Models.Queries.ParameterQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bulk_sender_backend.Controllers
{
    public class ParameterController : Controller
    {
        private readonly IMediator _mediator;
        public ParameterController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("api/parameter/{id}")]
        public async  Task<IActionResult> GetParameter(Guid id)
        {
            GetParameterQuery query = new GetParameterQuery(id);
            ParameterDTO parameter = await _mediator.Send(query);

            return Ok(parameter);
        }

        [HttpGet]
        [Route("api/parameters")]
        public async Task<IActionResult> GetAllParameters()
        {
            GetAllParametersQuery query = new GetAllParametersQuery();
            List<ParameterDTO> parameters = await _mediator.Send(query);

            return Ok(parameters);
        }

        [HttpGet]
        [Route("api/parameters/names")]
        public async Task<IActionResult> GetParametersNames()
        {
            GetParametersNamesQuery query = new GetParametersNamesQuery();
            List<string> parametersNames = await _mediator.Send(query);

            return Ok(parametersNames);
        }

        [HttpPost]
        [Route("api/parameter")]
        public async Task<IActionResult> AddParameter([FromBody] ParameterDTO parameter)
        {
            AddParameterCommand command = new AddParameterCommand(parameter);
            Guid parameterId = await _mediator.Send(command);
            return Ok(parameterId);
        }

        [HttpPut]
        [Route("api/parameter/edit/{id}")]
        public async Task<IActionResult> UpdateParameter(Guid id, [FromBody] EditParameterCommand command)
        {
            command.Parameter.Id = id;
            Guid parameterId = await _mediator.Send(command);

            return Ok(parameterId);
        }

        [HttpDelete]
        [Route("api/parameter/delete/{name}")]
        public async Task<IActionResult> DeleteParameter(string name)
        {
            DeleteParameterCommand command = new DeleteParameterCommand(name);
            int deletedCount = await _mediator.Send(command);

            return Ok(deletedCount);
        }
    }
}
