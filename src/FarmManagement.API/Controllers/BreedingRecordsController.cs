using FarmManagement.Application.Features.BreedingRecords.Commands;
using FarmManagement.Application.Features.BreedingRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedingRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BreedingRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all breeding records
        /// </summary>
        /// <param name="includeRelatedData">Include animal and sire data</param>
        [HttpGet]
        public async Task<IActionResult> GetBreedingRecords([FromQuery] bool includeRelatedData = false)
        {
            var result = await _mediator.Send(new GetAllBreedingRecordsQuery { IncludeRelatedData = includeRelatedData });
            return Ok(result);
        }

        /// <summary>
        /// Get breeding record by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBreedingRecordById(int id)
        {
            var result = await _mediator.Send(new GetBreedingRecordByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new { message = $"BreedingRecord with ID {id} not found" });
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new breeding record
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateBreedingRecord([FromBody] CreateBreedingRecordCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBreedingRecordById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing breeding record
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBreedingRecord(int id, [FromBody] UpdateBreedingRecordCommand command)
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
        /// Delete a breeding record
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBreedingRecord(int id)
        {
            var result = await _mediator.Send(new DeleteBreedingRecordCommand { Id = id });
            if (!result)
            {
                return NotFound(new { message = $"BreedingRecord with ID {id} not found" });
            }
            return NoContent();
        }
    }
}
