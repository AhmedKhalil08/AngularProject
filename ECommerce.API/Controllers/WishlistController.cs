using ECommerce.Application.Features.Wishlists.Commands.ClearWishList;
using ECommerce.Application.Features.Wishlists.Commands.CreateWishlist;
using ECommerce.Application.Features.Wishlists.Commands.DeleteWishlist;
using ECommerce.Application.Features.Wishlists.Queries.GetAllWishlists;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WishlistController(IMediator mediator )
        {
            _mediator = mediator;
        }

        #region Get All 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllWishlistsQuery());
            return Ok(result);
        }
        #endregion

        #region Add 
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddToWishlistCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region Remove 

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _mediator.Send(new DeleteWishlistCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion


        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAll()
        {
            var result = await _mediator.Send(new ClearWishlistCommand());
            return NoContent();
        }
    }
}
