using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.Application.Features.Orders.Commands.DeleteOrder;
using ECommerce.Application.Features.Orders.Commands.UpdateOrder;
using ECommerce.Application.Features.Orders.Queries.GetAllOrders;
using ECommerce.Application.Features.Orders.Queries.GetMyOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("MyOrder")]
        public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
        {
            return Ok(await Mediator.Send(new GetMyOrdersQuery()));
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("AllOrders")]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
        {
            return Ok(await Mediator.Send(new GetAllOrdersQuery()));
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderCommand command)
        {
            if (id != command.Id)
                return BadRequest("Bad Request For ID Conflict");

            var result = await Mediator.Send(command);

            if (result == null)
                return NotFound(new { message = "Order Not Found." });

            return Ok(new { message = "Order Changed Successfully" });
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await Mediator.Send(new DeleteOrderCommand { Id = id });

            if (!result)
                return NotFound(new { message = "This order doesn't Exist" });

            return Ok(new { message = "The order is deleted " });
        }
    }
}
