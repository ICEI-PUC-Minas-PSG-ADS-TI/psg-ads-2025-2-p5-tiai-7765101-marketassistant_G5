using MarketAPI.Contracts.Products;

namespace MarketAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductResponse>> GetOfficialAsync(CancellationToken cancellationToken = default);
        Task<ProductResponse?> GetOfficialByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ProductResponse> CreateOfficialAsync(OfficialProductCreateRequest request, CancellationToken cancellationToken = default);
        Task<ProductResponse?> UpdateOfficialAsync(Guid id, OfficialProductUpdateRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteOfficialAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductResponse>> GetCustomAsync(CancellationToken cancellationToken = default);
        Task<ProductResponse?> GetCustomByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ProductResponse> CreateCustomAsync(CustomProductCreateRequest request, CancellationToken cancellationToken = default);
        Task<ProductResponse?> UpdateCustomAsync(Guid id, CustomProductUpdateRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteCustomAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
