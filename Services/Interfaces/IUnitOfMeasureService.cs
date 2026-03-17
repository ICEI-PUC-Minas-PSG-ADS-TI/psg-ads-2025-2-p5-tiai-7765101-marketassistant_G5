using MarketAPI.Contracts.Units;

namespace MarketAPI.Services.Interfaces
{
    public interface IUnitOfMeasureService
    {
        Task<IReadOnlyList<UnitOfMeasureResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UnitOfMeasureResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UnitOfMeasureResponse> CreateAsync(UnitOfMeasureCreateRequest request, CancellationToken cancellationToken = default);
        Task<UnitOfMeasureResponse?> UpdateAsync(Guid id, UnitOfMeasureUpdateRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
