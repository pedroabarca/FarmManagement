using FarmManagement.Application.Features.Animals.Commands;
using FarmManagement.Application.Features.Animals.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnimalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all animals
        /// </summary>
        /// <param name="includeRelatedData">Include weight records, birth records, breeding records, and health records</param>
        [HttpGet]
        public async Task<IActionResult> GetAnimals([FromQuery] bool includeRelatedData = false)
        {
            var result = await _mediator.Send(new GetAllAnimalsQuery { IncludeRelatedData = includeRelatedData });
            return Ok(result);
        }

        /// <summary>
        /// Get animal by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimalById(int id)
        {
            var result = await _mediator.Send(new GetAnimalByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new { message = $"Animal with ID {id} not found" });
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new animal
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAnimalCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAnimalById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing animal
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] UpdateAnimalCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { message = "ID in URL does not match ID in request body" });
            }

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an animal
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var result = await _mediator.Send(new DeleteAnimalCommand { Id = id });
            if (!result)
            {
                return NotFound(new { message = $"Animal with ID {id} not found" });
            }
            return NoContent();
        }
    }
}
