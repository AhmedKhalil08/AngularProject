using ECommerce.Application.Features.Carts.Commands.ClearCart;
using ECommerce.Application.Features.Carts.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetCartByUserIdQuery ());
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            var result = await _mediator.Send(new ClearCartCommand ());
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
