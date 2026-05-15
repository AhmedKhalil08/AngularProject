using ECommerce.Application.Features.SellerProfiles.Commands.CreateSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.DeleteSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.UpdateSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Queries.GetMySellerProfile;
using ECommerce.Application.Features.SellerProfiles.Queries.GetSellerProfileById;
using ECommerce.Application.Features.SellerProfiles.Queries.GetSellerStats;
using ECommerce.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SellerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SellerController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Seller))]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSellerProfileByIdQuery { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateSellerProfileCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Seller))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateSellerProfileCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Seller))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSellerProfileCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize(Roles = nameof(UserRole.Seller))]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _mediator.Send(new GetMySellerProfileQuery());
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("stats")]
        [Authorize(Roles = nameof(UserRole.Seller))]
        public async Task<IActionResult> GetMyStats()
        {
            var result = await _mediator.Send(new GetSellerStatsQuery());
            return Ok(result);
        }

    }
}
