using ECommerce.Application.Features.Wishlists.Commands.CreateWishlist;
using ECommerce.Application.Features.Wishlists.Commands.DeleteWishlist;
using ECommerce.Application.Features.Wishlists.Queries.GetAllWishlists;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WishlistController(IMediator mediator)
        {
            _mediator=mediator;
        }

        #region Get All 
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string UserId)
        {
            var result = await _mediator.Send(new GetAllWishlistsQuery { UserId = UserId });
            return Ok(result);
        }
        #endregion

        #region Add 
        [HttpPost]
        public async Task <IActionResult> Add([FromBody] AddToWishlistCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region Remove 

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _mediator.Send(new DeleteWishlistCommand {  Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion
    }
}
