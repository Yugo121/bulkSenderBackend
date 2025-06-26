using Application.Models.Commands.ProductsCommands;
using Application.Models.DTO_s;
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
            GetManyProductsQuery query = new GetManyProductsQuery(quantity, page);
            List<ProductDTO> products = await _mediator.Send(query);

            return Ok(products);
        }
        [HttpGet]
        [Route("api/products/search/notInBl/{page}/{quantity}")]
        public async Task<IActionResult> GetProductsNotInBaselinker(int page, int quantity)
        {
            GetProductsNotAddedToBaselinkerQuery query = new GetProductsNotAddedToBaselinkerQuery(page, quantity);
            List<ProductDTO> products = await _mediator.Send(query);

            return Ok(products);
        }

        [HttpGet]
        [Route("api/products/search/notInBl/all")]
        public async Task<IActionResult> GetProductsNotInBaselinkerCount()
        {
            GetProductsNotAddedToBlCount query = new GetProductsNotAddedToBlCount();
            int count = await _mediator.Send(query);

            return Ok(count);
        }

        [HttpPost]
        [Route("api/product")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO product)
        {
            Guid productId = await _mediator.Send(new AddProductCommand(product));
            return Ok(productId);
        }
        [HttpPost]
        [Route("api/products/csv")]
        public async Task<IActionResult> AddProductsByCsv([FromBody] CsvImportRequest request)
        {
            await _mediator.Send(new ImportCsvCommand(request));
            return Ok(request);
        }
        [HttpPut]
        [Route("api/product/edit/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO product)
        {
            product.Id = id;
            Guid productId = await _mediator.Send(new EditProductCommand(product));
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
        [HttpPost]
        [Route("api/products/addMany")]
        public async Task<IActionResult> AddManyProducts([FromBody] List<ProductDTO> products)
        {
            AddManyProductsCommand command = new AddManyProductsCommand(products);
            List<Guid> productIds = await _mediator.Send(command);
            return Ok(productIds);
        }
    }
}
