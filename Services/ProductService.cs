using MarketAPI.Contracts.Products;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using MarketAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IOfficialProductRepository _officialRepository;
        private readonly ICustomProductRepository _customRepository;
        private readonly IRepository<UnitOfMeasure> _unitRepository;

        public ProductService(
            IOfficialProductRepository officialRepository,
            ICustomProductRepository customRepository,
            IRepository<UnitOfMeasure> unitRepository
        )
        {
            _officialRepository = officialRepository;
            _customRepository = customRepository;
            _unitRepository = unitRepository;
        }

        public async Task<IReadOnlyList<ProductResponse>> GetOfficialAsync(CancellationToken cancellationToken = default)
        {
            var products = await _officialRepository.Query()
                .Include(p => p.UnitOfMeasure)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            // FIXME: Gabriel, I have adjusted this code because it was throwing and exception in compilation time
            return products.Select(MapProductResponse).ToList();
        }

        public async Task<ProductResponse?> GetOfficialByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _officialRepository.Query()
                .Include(p => p.UnitOfMeasure)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product is null ? null : MapProductResponse(product);
        }

        public async Task<ProductResponse> CreateOfficialAsync(OfficialProductCreateRequest request, CancellationToken cancellationToken = default)
        {
            Normalize(request);

            var existing = await _officialRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existing is not null)
            {
                throw new InvalidOperationException("Official product already exists.");
            }

            await EnsureUnitExistsAsync(request.UnitOfMeasureId, cancellationToken);

            var product = new OfficialProduct
            {
                Name = request.Name,
                Description = request.Description,
                UnitOfMeasureId = request.UnitOfMeasureId,
                ImageUrl = request.ImageUrl,
                Barcode = request.Barcode
            };

            await _officialRepository.AddAsync(product, cancellationToken);
            await _officialRepository.SaveChangesAsync(cancellationToken);

            return await GetOfficialByIdAsync(product.Id, cancellationToken)
                ?? throw new InvalidOperationException("Failed to load created product.");
        }

        public async Task<ProductResponse?> UpdateOfficialAsync(Guid id, OfficialProductUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var product = await _officialRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return null;
            }

            Normalize(request);

            if (!string.Equals(product.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _officialRepository.GetByNameAsync(request.Name, cancellationToken);
                if (existing is not null)
                {
                    throw new InvalidOperationException("Official product already exists.");
                }
            }

            await EnsureUnitExistsAsync(request.UnitOfMeasureId, cancellationToken);

            product.Name = request.Name;
            product.Description = request.Description;
            product.UnitOfMeasureId = request.UnitOfMeasureId;
            product.ImageUrl = request.ImageUrl;
            product.Barcode = request.Barcode;

            _officialRepository.Update(product);
            await _officialRepository.SaveChangesAsync(cancellationToken);

            return await GetOfficialByIdAsync(product.Id, cancellationToken);
        }

        public async Task<bool> DeleteOfficialAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _officialRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return false;
            }

            _officialRepository.Remove(product);
            await _officialRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IReadOnlyList<ProductResponse>> GetCustomAsync(CancellationToken cancellationToken = default)
        {
            var products = await _customRepository.Query()
                .Include(p => p.UnitOfMeasure)
                .OrderBy(p => p.Name)                
                .ToListAsync(cancellationToken);
            
            // FIXME: Gabriel, I have adjusted this code because it was throwing and exception in compilation time
            return products.Select(MapProductResponse).ToList();
        }

        public async Task<ProductResponse?> GetCustomByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _customRepository.Query()
                .Include(p => p.UnitOfMeasure)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product is null ? null : MapProductResponse(product);
        }

        public async Task<ProductResponse> CreateCustomAsync(CustomProductCreateRequest request, CancellationToken cancellationToken = default)
        {
            Normalize(request);

            var existing = await _customRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existing is not null)
            {
                throw new InvalidOperationException("Custom product already exists.");
            }

            await EnsureUnitExistsAsync(request.UnitOfMeasureId, cancellationToken);

            var product = new CustomProduct
            {
                Name = request.Name,
                Description = request.Description,
                UnitOfMeasureId = request.UnitOfMeasureId
            };

            await _customRepository.AddAsync(product, cancellationToken);
            await _customRepository.SaveChangesAsync(cancellationToken);

            return await GetCustomByIdAsync(product.Id, cancellationToken)
                ?? throw new InvalidOperationException("Failed to load created product.");
        }

        public async Task<ProductResponse?> UpdateCustomAsync(Guid id, CustomProductUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var product = await _customRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return null;
            }

            Normalize(request);

            if (!string.Equals(product.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _customRepository.GetByNameAsync(request.Name, cancellationToken);
                if (existing is not null)
                {
                    throw new InvalidOperationException("Custom product already exists.");
                }
            }

            await EnsureUnitExistsAsync(request.UnitOfMeasureId, cancellationToken);

            product.Name = request.Name;
            product.Description = request.Description;
            product.UnitOfMeasureId = request.UnitOfMeasureId;

            _customRepository.Update(product);
            await _customRepository.SaveChangesAsync(cancellationToken);

            return await GetCustomByIdAsync(product.Id, cancellationToken);
        }

        public async Task<bool> DeleteCustomAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _customRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return false;
            }

            _customRepository.Remove(product);
            await _customRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        private void Normalize(OfficialProductCreateRequest request)
        {
            request.Name = request.Name.Trim();
            request.Description = request.Description?.Trim();
            request.ImageUrl = request.ImageUrl?.Trim();
            request.Barcode = request.Barcode?.Trim();
        }

        private void Normalize(OfficialProductUpdateRequest request)
        {
            request.Name = request.Name.Trim();
            request.Description = request.Description?.Trim();
            request.ImageUrl = request.ImageUrl?.Trim();
            request.Barcode = request.Barcode?.Trim();
        }

        private void Normalize(CustomProductCreateRequest request)
        {
            request.Name = request.Name.Trim();
            request.Description = request.Description?.Trim();
        }

        private void Normalize(CustomProductUpdateRequest request)
        {
            request.Name = request.Name.Trim();
            request.Description = request.Description?.Trim();
        }

        private async Task EnsureUnitExistsAsync(Guid? unitId, CancellationToken cancellationToken)
        {
            if (unitId is null)
            {
                return;
            }

            var exists = await _unitRepository.Query()
                .AnyAsync(u => u.Id == unitId.Value, cancellationToken);

            if (!exists)
            {
                throw new InvalidOperationException("Unit of measure not found.");
            }
        }

        private static ProductResponse MapProductResponse(CustomProduct product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitOfMeasureId = product.UnitOfMeasureId,
                UnitOfMeasureName = product.UnitOfMeasure?.Name,
                UnitOfMeasureAbbreviation = product.UnitOfMeasure?.Abbreviation
            };
        }

        private static ProductResponse MapProductResponse(OfficialProduct product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitOfMeasureId = product.UnitOfMeasureId,
                UnitOfMeasureName = product.UnitOfMeasure?.Name,
                UnitOfMeasureAbbreviation = product.UnitOfMeasure?.Abbreviation,
                ImageUrl = product.ImageUrl,
                Barcode = product.Barcode
            };
        }
    }
}
