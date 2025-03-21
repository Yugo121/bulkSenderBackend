using Application.Models.Commands.ProductsCommands;
using Application.Models.DTOs;
using Application.Models.Queries.ProductQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bulk_sender_backend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("api/product/{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            GetProductQuery query = new GetProductQuery(id);

            ProductDTO product = await _mediator.Send(query);
            
            return Ok(product);
        }

        [HttpGet]
        [Route("api/products/{page}/{quantity}")]
        public async Task<IActionResult> GetManyProducts(int page, int quantity)
        {
            GetManyProductsQuery query = new GetManyProductsQuery(page, quantity);
            List<ProductDTO> products = await _mediator.Send(query);

            return Ok(products);
        }

        [HttpPost]
        [Route("api/product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            Guid productId = await _mediator.Send(command);
            return Ok(productId);
        }
        [HttpPut]
        [Route("api/product/edit/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] EditProductCommand command)
        {
            command.Product.Id = id;
            Guid productId = await _mediator.Send(command);
            return Ok(productId);
        }
        [HttpDelete]
        [Route("api/product/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            DeleteProductCommand command = new DeleteProductCommand(id);
            Guid productId = await _mediator.Send(command);
            return Ok(productId);
        }
    }
}
