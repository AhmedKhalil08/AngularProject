using ECommerce.Application.Features.NewsLetter.Commands.Subscribe;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsletterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeNewsletterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { message = "Subscribed successfully!" });
        }
    }
}
