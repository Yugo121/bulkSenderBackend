using Application.Models.Commands.BrandCommands;
using Application.Models.DTOs;
using Application.Models.Queries.BrandQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bulk_sender_backend.Controllers
{
    public class BrandController : Controller
    {
        private readonly IMediator _mediator;
        public BrandController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        [HttpGet]
        [Route("api/brand/{id}")]
        public async Task<IActionResult> GetBrand(Guid id)
        {
            GetBrandQuery query = new GetBrandQuery(id);
            BrandDTO brand = await _mediator.Send(query);
            return Ok(brand);
        }

        [HttpGet]
        [Route("api/brands")]
        public async Task<IActionResult> GetAllBrands()
        {

           GetAllBrandsQuery query = new GetAllBrandsQuery();
            List<BrandDTO> brands = await _mediator.Send(query);
            return Ok(brands);
        }

        [HttpGet]
        [Route("api/brands/names")]
        public async Task<IActionResult> GetBrandsNames()
        {
            GetBrandsNamesQuery query = new GetBrandsNamesQuery();
            List<string> brandsNames = await _mediator.Send(query);
            return Ok(brandsNames);
        }

        [HttpPost]
        [Route("api/brand")]
        public async Task<IActionResult> AddBrand([FromBody] AddBrandCommand command)
        {
            Guid brandId = await _mediator.Send(command);
            return Ok(brandId);
        }

        [HttpPut]
        [Route("api/brand/edit/{id}")]
        public async Task<IActionResult> UpdateBrand(Guid id, [FromBody] EditBrandCommand command)
        {
            command.Brand.Id = id;
            Guid brandId = await _mediator.Send(command);
            return Ok(brandId);
        }

        [HttpDelete]
        [Route("api/brand/delete/{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            DeleteBrandCommand command = new DeleteBrandCommand(id);
            Guid brandId = await _mediator.Send(command);
            return Ok(brandId);
        }
    }
}
