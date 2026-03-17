using MarketAPI.Contracts.Units;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using MarketAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Services
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfMeasureRepository _unitRepository;

        public UnitOfMeasureService(IUnitOfMeasureRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<IReadOnlyList<UnitOfMeasureResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var units = await _unitRepository.Query()
                .OrderBy(u => u.Name)
                .Select(u => new UnitOfMeasureResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Abbreviation = u.Abbreviation
                })
                .ToListAsync(cancellationToken);

            return units;
        }

        public async Task<UnitOfMeasureResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var unit = await _unitRepository.GetByIdAsync(id, cancellationToken);
            if (unit is null)
            {
                return null;
            }

            return new UnitOfMeasureResponse
            {
                Id = unit.Id,
                Name = unit.Name,
                Abbreviation = unit.Abbreviation
            };
        }

        public async Task<UnitOfMeasureResponse> CreateAsync(UnitOfMeasureCreateRequest request, CancellationToken cancellationToken = default)
        {
            request.Name = request.Name.Trim();
            request.Abbreviation = request.Abbreviation.Trim();

            var existing = await _unitRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existing is not null)
            {
                throw new InvalidOperationException("Unit of measure already exists.");
            }

            var unit = new UnitOfMeasure
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation
            };

            await _unitRepository.AddAsync(unit, cancellationToken);
            await _unitRepository.SaveChangesAsync(cancellationToken);

            return new UnitOfMeasureResponse
            {
                Id = unit.Id,
                Name = unit.Name,
                Abbreviation = unit.Abbreviation
            };
        }

        public async Task<UnitOfMeasureResponse?> UpdateAsync(Guid id, UnitOfMeasureUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var unit = await _unitRepository.GetByIdAsync(id, cancellationToken);
            if (unit is null)
            {
                return null;
            }

            request.Name = request.Name.Trim();
            request.Abbreviation = request.Abbreviation.Trim();

            if (!string.Equals(unit.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _unitRepository.GetByNameAsync(request.Name, cancellationToken);
                if (existing is not null)
                {
                    throw new InvalidOperationException("Unit of measure already exists.");
                }
            }

            unit.Name = request.Name;
            unit.Abbreviation = request.Abbreviation;

            _unitRepository.Update(unit);
            await _unitRepository.SaveChangesAsync(cancellationToken);

            return new UnitOfMeasureResponse
            {
                Id = unit.Id,
                Name = unit.Name,
                Abbreviation = unit.Abbreviation
            };
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var unit = await _unitRepository.GetByIdAsync(id, cancellationToken);
            if (unit is null)
            {
                return false;
            }

            _unitRepository.Remove(unit);
            await _unitRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
