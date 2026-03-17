using MarketAPI.Services.Models;

namespace MarketAPI.Services.Interfaces
{
    public interface IGoogleTokenValidator
    {
        Task<GoogleTokenPayload?> ValidateIdTokenAsync(
            string idToken,
            CancellationToken cancellationToken = default
        );
    }
}
