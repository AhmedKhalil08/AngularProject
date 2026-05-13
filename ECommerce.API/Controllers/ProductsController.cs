using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Commands.DeleteProduct;
using ECommerce.Application.Features.Products.Commands.UpdateProduct;
using ECommerce.Application.Features.Products.Queries.GetAllProducts;
using ECommerce.Application.Features.Products.Queries.GetMyProducts;
using ECommerce.Application.Features.Products.Queries.GetProductById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            // Implement logic to retrieve products
            return Ok(await Mediator.Send(new GetAllProductsQuery()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }
        [Authorize(Roles = "Seller")]
        [HttpGet("MyProducts")]
        public async Task<ActionResult<List<ProductDto>>> GetMyProdcuts()
        {
            return Ok(await Mediator.Send(new GetMyProductsQuery()));
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await Mediator.Send(command);

            if (result == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteProductCommand { Id = id });
            if (!result)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return NoContent();
        }

    }
}
