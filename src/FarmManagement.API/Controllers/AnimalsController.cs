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

        [HttpGet]
        public async Task<IActionResult> GetAnimals()
        {
            var result = await _mediator.Send(new GetAllAnimalsQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAnimalCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAnimals), new { id = result.Id }, result);
        }
    }
}
