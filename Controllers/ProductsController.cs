using MarketAPI.Contracts.Products;
using MarketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet("official")]
        public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetOfficial(
            CancellationToken cancellationToken
        )
        {
            var products = await _productService.GetOfficialAsync(cancellationToken);
            return Ok(products);
        }

        [Authorize]
        [HttpGet("official/{id:guid}")]
        public async Task<ActionResult<ProductResponse>> GetOfficialById(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var product = await _productService.GetOfficialByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Authorize]
        [HttpPost("official")]
        public async Task<ActionResult<ProductResponse>> CreateOfficial(
            [FromBody] OfficialProductCreateRequest request,
            CancellationToken cancellationToken
        )
        {
            var created = await _productService.CreateOfficialAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetOfficialById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("official/{id:guid}")]
        public async Task<ActionResult<ProductResponse>> UpdateOfficial(
            Guid id,
            [FromBody] OfficialProductUpdateRequest request,
            CancellationToken cancellationToken
        )
        {
            var updated = await _productService.UpdateOfficialAsync(id, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("official/{id:guid}")]
        public async Task<IActionResult> DeleteOfficial(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var deleted = await _productService.DeleteOfficialAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("custom")]
        public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetCustom(
            CancellationToken cancellationToken
        )
        {
            var products = await _productService.GetCustomAsync(cancellationToken);
            return Ok(products);
        }

        [Authorize]
        [HttpGet("custom/{id:guid}")]
        public async Task<ActionResult<ProductResponse>> GetCustomById(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var product = await _productService.GetCustomByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Authorize]
        [HttpPost("custom")]
        public async Task<ActionResult<ProductResponse>> CreateCustom(
            [FromBody] CustomProductCreateRequest request,
            CancellationToken cancellationToken
        )
        {
            var created = await _productService.CreateCustomAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetCustomById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("custom/{id:guid}")]
        public async Task<ActionResult<ProductResponse>> UpdateCustom(
            Guid id,
            [FromBody] CustomProductUpdateRequest request,
            CancellationToken cancellationToken
        )
        {
            var updated = await _productService.UpdateCustomAsync(id, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("custom/{id:guid}")]
        public async Task<IActionResult> DeleteCustom(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var deleted = await _productService.DeleteCustomAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
