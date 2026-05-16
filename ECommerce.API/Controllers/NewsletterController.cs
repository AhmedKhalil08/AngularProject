using ECommerce.Application.Features.NewsLetter.Commands.Subscribe;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INewsletterRepository _repository;

        public NewsletterController(IMediator mediator, INewsletterRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeNewsletterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { message = "Subscribed successfully!" });
        }
        [HttpGet("subscribers")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetSubscribers()
        {
            var subscribers = await _repository.GetAllAsync();
            return Ok(subscribers);
        }
    }
}
