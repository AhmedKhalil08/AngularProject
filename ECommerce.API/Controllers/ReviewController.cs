using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Reviews.Commands.CreateReview;
using ECommerce.Application.Features.Reviews.Commands.DeleteReview;
using ECommerce.Application.Features.Reviews.Queries.GetAllReviews;
using ECommerce.Application.Features.Reviews.Queries.GetReviewById;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ApiControllerBase
    {
        private IMediator _mediator;
        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Create Review
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateReviewCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);

        }

        //Delete Review
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteReviewCommand { Id = id });
            if (!result)
            {
                return NotFound($"Review with ID {id} not found.");
            }
            return NoContent();
        }


        ////Update Review
        //[HttpPost("{id}")]
        //public async Task<IActionResult> Update (int id, [FromForm] UpdateReviewCommand command)
        //{

        //}



        //Get All Reviews
        [HttpGet]
        public async Task<ActionResult<List<ReviewDto>>> GetAllReviews()
        {
            return Ok(await Mediator.Send(new GetAllReviewsQuery()));
        }



        //Get Review By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReviewById(int id)
        {
            return Ok(await Mediator.Send(new GetReviewByIdQuery { Id = id }));
        }




    }
}
