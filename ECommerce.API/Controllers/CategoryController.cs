using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Categories.Commands.CreateCategory;
using ECommerce.Application.Features.Categories.Commands.DeleteCategory;
using ECommerce.Application.Features.Categories.Commands.UpdateCategory;
using ECommerce.Application.Features.Categories.Queries.GetAllCategories;
using ECommerce.Application.Features.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApiControllerBase
    {
        private IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Create Category

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);

        }

        //DeleteCategory
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteCategoryCommand { Id = id });
            if (!result)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            return NoContent();
        }
        //Update Category
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var result = await Mediator.Send(command);

            if (result == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }


            return Ok(result);
        }

        //Get All Categories

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            return Ok(await Mediator.Send(new GetAllCategoriesQuery()));
        }



        //Get Banner By Id
        [HttpGet("id/{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            return Ok(await Mediator.Send(new GetCategoryByIdQuery { Id = id }));
        }

        //Get Category By Name
        [HttpGet("name/{name}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryByName(string name)
        {
            List<CategoryDto> ctg = new List<CategoryDto>();
            ctg = await Mediator.Send(new GetAllCategoriesQuery());
            var result = ctg.Where(c => c.Name == name).FirstOrDefault();
            return Ok(result);
        }

        //Get Category by Product
        //Get Subcategories by Category
        //Get Parent Category by Subcategory

    }
}
