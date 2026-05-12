using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Banners.Commands.CreateBanner;
using ECommerce.Application.Features.Banners.Commands.DeleteBanner;
using ECommerce.Application.Features.Banners.Commands.UpdateBanner;
using ECommerce.Application.Features.Banners.Queries.GetAllBanners;
using ECommerce.Application.Features.Banners.Queries.GetBannerById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ApiControllerBase
    {
        private IMediator _mediator;
        public BannerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Create Banner
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBannerCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);

        }

        //Delete Banner
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteBannerCommand { Id = id });
            if (!result)
            {
                return NotFound($"Banner with ID {id} not found.");
            }
            return NoContent();
        }
        //Update Banner
        [HttpGet("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var result = await Mediator.Send(new UpdateBannerCommand { Id = id });

            if (result == null)
            {
                return NotFound($"Banner with ID {id} not found.");
            }


            return Ok(result);
        }

        //Get All Banners

        [HttpGet]
        public async Task<ActionResult<List<BannerDto>>> GetAllBanners()
        {
            return Ok(await Mediator.Send(new GetAllBannersQuery()));
        }



        //Get Banner By Id
        [HttpGet("get/{id}")]
        public async Task<ActionResult<BannerDto>> GetBannerById(int id)
        {
            return Ok(await Mediator.Send(new GetBannerByIdQuery { Id = id }));
        }


    }
}
