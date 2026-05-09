using ECommerce.Application.Features.SellerProfiles.Commands.ApproveSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.CreateSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.DeleteSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.UpdateSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Queries.GetAllSellerProfiles;
using ECommerce.Application.Features.SellerProfiles.Queries.GetSellerProfileById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SellerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Get All 
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var result = await _mediator.Send(new GetAllSellerProfilesQuery());
            return Ok(result);
        }
        #endregion

        #region Get By Id 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSellerProfileByIdQuery { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }
        #endregion

        #region Create

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSellerProfileCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region Update

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , UpdateSellerProfileCommand command)
        {
            command.Id= id;
            var result= await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send( new DeleteSellerProfileCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion

        #region Approve 
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id , [FromBody]ApproveSellerProfileCommand command)
        {
            command.Id= id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion
    }
}
