using ECommerce.Application.Features.CartItems.Commands.DeleteCartItem;
using ECommerce.Application.Features.CartItems.Commands.UpdateCartItem;
using ECommerce.Application.Features.Carts.Commands.AddToCart;
using ECommerce.Application.Features.Carts.Commands.ClearCart;
using ECommerce.Application.Features.Carts.Commands.SyncCart;
using ECommerce.Application.Features.Carts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var result = await _mediator.Send(new GetCartByUserIdQuery());
            //if (result == null) return NotFound();
            return Ok(result);
        }



        [HttpPost("add-item")]

        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok(new { Success = true, Message = "Item added to cart successfully." });
                }

                return BadRequest(new { Success = false, Message = "Failed to add item to cart." });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
        [HttpPost("sync")]
        public async Task<IActionResult> Sync([FromBody] SyncCartCommand command) => Ok(await _mediator.Send(command));


        [HttpDelete("items/{id}")]
        public async Task<IActionResult> Remove(int id) => Ok(await _mediator.Send(new DeleteCartItemCommand { CartItemId = id }));
        [HttpPut("update-item")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = "Cart updated successfully.", Success = result });
            }
            catch (Exception ex)
            {
                // In production, you might want to return a more structured error response
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpDelete("clear")]
        public async Task<IActionResult> Clear() => Ok(await _mediator.Send(new ClearCartCommand()));
    }

}
