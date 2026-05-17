using ECommerce.Application.DTOs;
using ECommerce.Application.Features.PromoCodes.Commands.CreatePromoCode;
using ECommerce.Application.Features.PromoCodes.Commands.DeletePromoCode;
using ECommerce.Application.Features.PromoCodes.Commands.UpdatePromoCode;
using ECommerce.Application.Features.PromoCodes.Queries.GetAllPromoCodes;
using ECommerce.Application.Features.PromoCodes.Queries.GetPromoCodeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : ApiControllerBase
    {

        private IMediator _mediator;
        public PromoCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //Create Banner
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePromoCodeCommand command)
        {
            //var result =;

            return Ok(await Mediator.Send(command));

        }

        //Delete PromoCode
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Mediator.Send(new DeletePromoCodeCommand { Id = id });
            if (!result)
            {
                return NotFound($"PromoCode with ID {id} not found.");
            }
            return NoContent();
        }
        //Update PromoCode
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id ,UpdatePromoCodeCommand command )
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch.");
            }
            var result = await Mediator.Send(command);
            if (result == null)
            {
                return NotFound($"PromoCode with ID {id} not found.");
            }
            return Ok(result);
        }
        //Get All PromoCodes
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeDto>>> GetAllPromoCodes()
        {
            return Ok(await Mediator.Send(new GetAllPromoCodesQuery()));
        }

        //Get PromoCode By Id
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PromoCodeDto>> GetBannerById(int id)
        {
            return Ok(await Mediator.Send(new GetPromoCodeByIdQuery { Id = id }));
        }
        //Create PromoCode

        //Get PromoCode By Code

    }
}
