using ECommerce.Application.Features.SellerProfiles.Commands.CreateSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.DeleteSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.UpdateSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Queries.GetSellerProfileById;
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSellerProfileCommand command)
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

    }
}
