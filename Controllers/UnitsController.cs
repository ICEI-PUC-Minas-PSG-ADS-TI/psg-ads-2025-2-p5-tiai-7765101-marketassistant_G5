using MarketAPI.Contracts.Units;
using MarketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Controllers
{
    [ApiController]
    [Route("units")]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitOfMeasureService _unitService;

        public UnitsController(IUnitOfMeasureService unitService)
        {
            _unitService = unitService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UnitOfMeasureResponse>>> GetAll(
            CancellationToken cancellationToken
        )
        {
            var units = await _unitService.GetAllAsync(cancellationToken);
            return Ok(units);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UnitOfMeasureResponse>> GetById(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var unit = await _unitService.GetByIdAsync(id, cancellationToken);
            if (unit is null)
            {
                return NotFound();
            }

            return Ok(unit);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UnitOfMeasureResponse>> Create(
            [FromBody] UnitOfMeasureCreateRequest request,
            CancellationToken cancellationToken
        )
        {
            var created = await _unitService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UnitOfMeasureResponse>> Update(
            Guid id,
            [FromBody] UnitOfMeasureUpdateRequest request,
            CancellationToken cancellationToken
        )
        {
            var updated = await _unitService.UpdateAsync(id, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var deleted = await _unitService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
