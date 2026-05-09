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

        #region Get  
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string UserId)
        {
            var result = await _mediator.Send(new GetCartByUserIdQuery { UserId = UserId });
            if (result == null) return NotFound();
            return Ok(result);
        }
        #endregion

        #region Clear Cart
        [HttpDelete]
        public async Task<IActionResult> Clear([FromQuery] string userId)
        {
            var result = await _mediator.Send(new ClearCartCommand { UserId = userId });
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion
    }
}
