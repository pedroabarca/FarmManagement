using FarmManagement.Application.Features.BirthRecords.Commands;
using FarmManagement.Application.Features.BirthRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BirthRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all birth records
        /// </summary>
        /// <param name="includeRelatedData">Include dam and calf data</param>
        [HttpGet]
        public async Task<IActionResult> GetBirthRecords([FromQuery] bool includeRelatedData = false)
        {
            var result = await _mediator.Send(new GetAllBirthRecordsQuery { IncludeRelatedData = includeRelatedData });
            return Ok(result);
        }

        /// <summary>
        /// Get birth record by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBirthRecordById(int id)
        {
            var result = await _mediator.Send(new GetBirthRecordByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new { message = $"BirthRecord with ID {id} not found" });
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new birth record
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateBirthRecord([FromBody] CreateBirthRecordCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBirthRecordById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing birth record
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBirthRecord(int id, [FromBody] UpdateBirthRecordCommand command)
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
        /// Delete a birth record
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBirthRecord(int id)
        {
            var result = await _mediator.Send(new DeleteBirthRecordCommand { Id = id });
            if (!result)
            {
                return NotFound(new { message = $"BirthRecord with ID {id} not found" });
            }
            return NoContent();
        }
    }
}
