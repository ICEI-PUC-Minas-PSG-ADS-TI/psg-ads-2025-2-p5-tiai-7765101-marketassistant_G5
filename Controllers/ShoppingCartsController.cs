using MarketAPI.Contracts.Carts;
using MarketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketAPI.Controllers
{
    [ApiController]
    [Route("carts")]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartsController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ShoppingCartResponse>>> GetAll(
            CancellationToken cancellationToken
        )
        {
            var userId = GetUserId();
            var carts = await _cartService.GetForUserAsync(userId, cancellationToken);
            return Ok(carts);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShoppingCartResponse>> GetById(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var cart = await _cartService.GetByIdAsync(id, cancellationToken);
            if (cart is null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ShoppingCartResponse>> Create(
            [FromBody] ShoppingCartCreateRequest request,
            CancellationToken cancellationToken
        )
        {
            var userId = GetUserId();
            var created = await _cartService.CreateAsync(userId, request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ShoppingCartResponse>> Update(
            Guid id,
            [FromBody] ShoppingCartUpdateRequest request,
            CancellationToken cancellationToken
        )
        {
            var userId = GetUserId();
            var updated = await _cartService.UpdateAsync(id, userId, request, cancellationToken);
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
            var userId = GetUserId();
            var deleted = await _cartService.DeleteAsync(id, userId, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("{cartId:guid}/items")]
        public async Task<ActionResult<CartItemResponse>> AddItem(
            Guid cartId,
            [FromBody] CartItemCreateRequest request,
            CancellationToken cancellationToken
        )
        {
            var userId = GetUserId();
            try
            {
                var created = await _cartService.AddItemAsync(userId, cartId, request, cancellationToken);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("items/{itemId:guid}")]
        public async Task<ActionResult<CartItemResponse>> GetItemById(
            Guid itemId,
            CancellationToken cancellationToken
        )
        {
            var userId = GetUserId();
            var item = await _cartService.GetItemByIdAsync(userId, itemId, cancellationToken);
            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [Authorize]
        [HttpPut("items/{itemId:guid}")]
        public async Task<ActionResult<CartItemResponse>> UpdateItem(
            Guid itemId,
            [FromBody] CartItemUpdateRequest request,
            CancellationToken cancellationToken
        )
        {
            var userId = GetUserId();
            var updated = await _cartService.UpdateItemAsync(userId, itemId, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("items/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(
            Guid itemId,
            CancellationToken cancellationToken
        )
        {
            var userId = GetUserId();
            var deleted = await _cartService.RemoveItemAsync(userId, itemId, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            return Guid.Parse(userId ?? throw new InvalidOperationException("User id not found."));
        }
    }
}
