using MarketAPI.Contracts.Carts;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using MarketAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IRepository<CartItem> _itemRepository;
        private readonly IRepository<OfficialProduct> _officialRepository;
        private readonly IRepository<CustomProduct> _customRepository;

        public ShoppingCartService(
            IShoppingCartRepository cartRepository,
            IRepository<CartItem> itemRepository,
            IRepository<OfficialProduct> officialRepository,
            IRepository<CustomProduct> customRepository
        )
        {
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
            _officialRepository = officialRepository;
            _customRepository = customRepository;
        }

        public async Task<IReadOnlyList<ShoppingCartResponse>> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var carts = await _cartRepository.GetByUserAsync(userId, cancellationToken);
            return carts.Select(MapCart).ToList();
        }

        public async Task<ShoppingCartResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepository.GetWithItemsAsync(id, cancellationToken);
            return cart is null ? null : MapCart(cart);
        }

        public async Task<ShoppingCartResponse> CreateAsync(Guid userId, ShoppingCartCreateRequest request, CancellationToken cancellationToken = default)
        {
            request.Name = request.Name.Trim();

            var cart = new ShoppingCart
            {
                Name = request.Name,
                CreatedByUserId = userId
            };

            await _cartRepository.AddAsync(cart, cancellationToken);
            await _cartRepository.SaveChangesAsync(cancellationToken);

            var created = await _cartRepository.GetWithItemsAsync(cart.Id, cancellationToken);
            return created is null ? throw new InvalidOperationException("Failed to load created cart.") : MapCart(created);
        }

        public async Task<ShoppingCartResponse?> UpdateAsync(Guid id, Guid userId, ShoppingCartUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepository.GetByIdAsync(id, cancellationToken);
            if (cart is null || cart.CreatedByUserId != userId)
            {
                return null;
            }

            request.Name = request.Name.Trim();
            cart.Name = request.Name;

            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync(cancellationToken);

            var updated = await _cartRepository.GetWithItemsAsync(cart.Id, cancellationToken);
            return updated is null ? null : MapCart(updated);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepository.GetByIdAsync(id, cancellationToken);
            if (cart is null || cart.CreatedByUserId != userId)
            {
                return false;
            }

            _cartRepository.Remove(cart);
            await _cartRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<CartItemResponse> AddItemAsync(Guid userId, Guid cartId, CartItemCreateRequest request, CancellationToken cancellationToken = default)
        {
            if ((request.OfficialProductId is null && request.CustomProductId is null)
                || (request.OfficialProductId is not null && request.CustomProductId is not null))
            {
                throw new InvalidOperationException("Cart item must reference exactly one product.");
            }

            var cart = await _cartRepository.GetByIdAsync(cartId, cancellationToken);
            if (cart is null || cart.CreatedByUserId != userId)
            {
                throw new InvalidOperationException("Shopping cart not found.");
            }

            OfficialProduct? officialProduct = null;
            CustomProduct? customProduct = null;

            if (request.OfficialProductId is not null)
            {
                officialProduct = await _officialRepository.Query()
                    .FirstOrDefaultAsync(p => p.Id == request.OfficialProductId.Value, cancellationToken);
                if (officialProduct is null)
                {
                    throw new InvalidOperationException("Official product not found.");
                }
            }

            if (request.CustomProductId is not null)
            {
                customProduct = await _customRepository.Query()
                    .FirstOrDefaultAsync(p => p.Id == request.CustomProductId.Value, cancellationToken);
                if (customProduct is null)
                {
                    throw new InvalidOperationException("Custom product not found.");
                }
            }

            var item = new CartItem(
                cartId,
                request.Quantity,
                request.OfficialProductId,
                request.CustomProductId
            );

            await _itemRepository.AddAsync(item, cancellationToken);
            await _itemRepository.SaveChangesAsync(cancellationToken);

            return new CartItemResponse
            {
                Id = item.Id,
                Quantity = item.Quantity,
                ShoppingCartId = item.ShoppingCartId,
                OfficialProductId = item.OfficialProductId,
                OfficialProductName = officialProduct?.Name,
                CustomProductId = item.CustomProductId,
                CustomProductName = customProduct?.Name
            };
        }

        public async Task<CartItemResponse?> GetItemByIdAsync(Guid userId, Guid itemId, CancellationToken cancellationToken = default)
        {
            var item = await _itemRepository.GetByIdAsync(itemId, cancellationToken);
            if (item is null)
            {
                return null;
            }

            var cart = await _cartRepository.GetByIdAsync(item.ShoppingCartId, cancellationToken);
            if (cart is null || cart.CreatedByUserId != userId)
            {
                return null;
            }

            string? officialProductName = null;
            string? customProductName = null;

            if (item.OfficialProductId is not null)
            {
                officialProductName = await _officialRepository.Query()
                    .Where(p => p.Id == item.OfficialProductId.Value)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            if (item.CustomProductId is not null)
            {
                customProductName = await _customRepository.Query()
                    .Where(p => p.Id == item.CustomProductId.Value)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            return new CartItemResponse
            {
                Id = item.Id,
                Quantity = item.Quantity,
                ShoppingCartId = item.ShoppingCartId,
                OfficialProductId = item.OfficialProductId,
                OfficialProductName = officialProductName,
                CustomProductId = item.CustomProductId,
                CustomProductName = customProductName
            };
        }

        public async Task<CartItemResponse?> UpdateItemAsync(Guid userId, Guid itemId, CartItemUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var item = await _itemRepository.GetByIdAsync(itemId, cancellationToken);
            if (item is null)
            {
                return null;
            }

            var cart = await _cartRepository.GetByIdAsync(item.ShoppingCartId, cancellationToken);
            if (cart is null || cart.CreatedByUserId != userId)
            {
                return null;
            }

            item.UpdateQuantity(request.Quantity);

            _itemRepository.Update(item);
            await _itemRepository.SaveChangesAsync(cancellationToken);

            string? officialProductName = null;
            string? customProductName = null;

            if (item.OfficialProductId is not null)
            {
                officialProductName = await _officialRepository.Query()
                    .Where(p => p.Id == item.OfficialProductId.Value)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            if (item.CustomProductId is not null)
            {
                customProductName = await _customRepository.Query()
                    .Where(p => p.Id == item.CustomProductId.Value)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            return new CartItemResponse
            {
                Id = item.Id,
                Quantity = item.Quantity,
                ShoppingCartId = item.ShoppingCartId,
                OfficialProductId = item.OfficialProductId,
                OfficialProductName = officialProductName,
                CustomProductId = item.CustomProductId,
                CustomProductName = customProductName
            };
        }

        public async Task<bool> RemoveItemAsync(Guid userId, Guid itemId, CancellationToken cancellationToken = default)
        {
            var item = await _itemRepository.GetByIdAsync(itemId, cancellationToken);
            if (item is null)
            {
                return false;
            }

            var cart = await _cartRepository.GetByIdAsync(item.ShoppingCartId, cancellationToken);
            if (cart is null || cart.CreatedByUserId != userId)
            {
                return false;
            }

            _itemRepository.Remove(item);
            await _itemRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static ShoppingCartResponse MapCart(ShoppingCart cart)
        {
            return new ShoppingCartResponse
            {
                Id = cart.Id,
                Name = cart.Name,
                CreatedByUserId = cart.CreatedByUserId,
                CreatedByUserName = cart.CreatedByUser?.Name ?? string.Empty,
                CreatedAt = cart.CreatedAt,
                Items = cart.Items.Select(item => new CartItemResponse
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    ShoppingCartId = item.ShoppingCartId,
                    OfficialProductId = item.OfficialProductId,
                    OfficialProductName = item.OfficialProduct?.Name,
                    CustomProductId = item.CustomProductId,
                    CustomProductName = item.CustomProduct?.Name
                }).ToList()
            };
        }
    }
}
