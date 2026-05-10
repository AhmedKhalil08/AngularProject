using ECommerce.Application.Features.Addresses.Commands.CreateAddress;
using ECommerce.Application.Features.Addresses.Commands.DeleteAddress;
using ECommerce.Application.Features.Addresses.Commands.UpdateAddress;
using ECommerce.Application.Features.Addresses.Queries.GetAddressById;
using ECommerce.Application.Features.Addresses.Queries.GetAllAddresses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AddressesController(IMediator mediator)
        {
            _mediator= mediator;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string userId)
        {
            var result = await _mediator.Send(new GetAllAddressesQuery { UserId= userId});
                return Ok(result);
        }
        #endregion

        #region  GetByID 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAddressByIdQuery { Id= id });
            if (result == null) return NotFound();
            return Ok(result);
        }
        #endregion

        #region Create Address
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        #endregion

        #region Update Address 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , [FromBody]UpdateAddressCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteAddressCommand { Id = id });
            if(!result)return NotFound();
            return NoContent();
        }
        #endregion
    }
}
