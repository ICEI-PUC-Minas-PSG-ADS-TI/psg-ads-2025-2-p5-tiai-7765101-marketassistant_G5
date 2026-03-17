using MarketAPI.Contracts.Auth;
using MarketAPI.Services.Models;

namespace MarketAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginWithGoogleAsync(
            string idToken,
            CancellationToken cancellationToken = default
        );

        Task<AuthResponse?> LoginWithGoogleAsync(
            GoogleTokenPayload payload,
            CancellationToken cancellationToken = default
        );
    }
}
