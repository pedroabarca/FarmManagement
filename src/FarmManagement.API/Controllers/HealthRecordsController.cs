using FarmManagement.Application.Features.HealthRecords.Commands;
using FarmManagement.Application.Features.HealthRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HealthRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all health records
        /// </summary>
        /// <param name="includeAnimal">Include related animal data</param>
        [HttpGet]
        public async Task<IActionResult> GetHealthRecords([FromQuery] bool includeAnimal = false)
        {
            var result = await _mediator.Send(new GetAllHealthRecordsQuery { IncludeAnimal = includeAnimal });
            return Ok(result);
        }

        /// <summary>
        /// Get health record by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHealthRecordById(int id)
        {
            var result = await _mediator.Send(new GetHealthRecordByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new { message = $"HealthRecord with ID {id} not found" });
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new health record
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateHealthRecord([FromBody] CreateHealthRecordCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetHealthRecordById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing health record
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHealthRecord(int id, [FromBody] UpdateHealthRecordCommand command)
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
        /// Delete a health record
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthRecord(int id)
        {
            var result = await _mediator.Send(new DeleteHealthRecordCommand { Id = id });
            if (!result)
            {
                return NotFound(new { message = $"HealthRecord with ID {id} not found" });
            }
            return NoContent();
        }
    }
}
