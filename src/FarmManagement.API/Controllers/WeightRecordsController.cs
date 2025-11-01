using FarmManagement.Application.Features.WeightRecords.Commands;
using FarmManagement.Application.Features.WeightRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeightRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all weight records
        /// </summary>
        /// <param name="includeAnimal">Include related animal data</param>
        [HttpGet]
        public async Task<IActionResult> GetWeightRecords([FromQuery] bool includeAnimal = false)
        {
            var result = await _mediator.Send(new GetAllWeightRecordsQuery { IncludeAnimal = includeAnimal });
            return Ok(result);
        }

        /// <summary>
        /// Get weight record by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeightRecordById(int id)
        {
            var result = await _mediator.Send(new GetWeightRecordByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new { message = $"WeightRecord with ID {id} not found" });
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new weight record
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateWeightRecord([FromBody] CreateWeightRecordCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetWeightRecordById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing weight record
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeightRecord(int id, [FromBody] UpdateWeightRecordCommand command)
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
        /// Delete a weight record
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightRecord(int id)
        {
            var result = await _mediator.Send(new DeleteWeightRecordCommand { Id = id });
            if (!result)
            {
                return NotFound(new { message = $"WeightRecord with ID {id} not found" });
            }
            return NoContent();
        }
    }
}
