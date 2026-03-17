using MarketAPI.Contracts.Auth;
using MarketAPI.Entities;
using MarketAPI.Options;
using MarketAPI.Repositories.Interfaces;
using MarketAPI.Services.Interfaces;
using MarketAPI.Services.Models;
using Microsoft.Extensions.Options;

namespace MarketAPI.Services
{
    public class AuthService : IAuthService
    {
        private const string GoogleProviderName = "google";
        private readonly IGoogleTokenValidator _googleTokenValidator;
        private readonly IUserRepository _userRepository;
        private readonly IUserProviderRepository _userProviderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ITokenService _tokenService;
        private readonly JwtOptions _jwtOptions;

        public AuthService(
            IGoogleTokenValidator googleTokenValidator,
            IUserRepository userRepository,
            IShoppingCartRepository shoppingCartRepository,
            IUserProviderRepository userProviderRepository,
            ITokenService tokenService,
            IOptions<JwtOptions> jwtOptions
        )
        {
            _googleTokenValidator = googleTokenValidator;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _userProviderRepository = userProviderRepository;
            _tokenService = tokenService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AuthResponse?> LoginWithGoogleAsync(
            string idToken,
            CancellationToken cancellationToken = default
        )
        {
            var payload = await _googleTokenValidator.ValidateIdTokenAsync(idToken, cancellationToken);
            
            if (payload is null)
                return null;

            return await LoginWithGoogleAsync(payload, cancellationToken);
        }

        public async Task<AuthResponse?> LoginWithGoogleAsync(
            GoogleTokenPayload payload,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(payload.Email) || string.IsNullOrWhiteSpace(payload.Subject))
                return null;

            var isNewUser = false;
            var isNewProvider = false;

            var user = await _userRepository.GetByEmailAsync(payload.Email, cancellationToken);
            if (user is null)
            {
                user = new User
                {
                    Name = string.IsNullOrWhiteSpace(payload.Name) ? "User" : payload.Name,
                    Email = payload.Email
                };

                await _userRepository.AddAsync(user, cancellationToken);

                var shoppingCart = new ShoppingCart
                {
                    Name = "default",
                    CreatedByUser = user,
                };

                await _shoppingCartRepository.AddAsync(shoppingCart, cancellationToken);

                isNewUser = true;
            }

            var existingProvider = await _userProviderRepository.GetByProviderUserIdAsync(
                GoogleProviderName,
                payload.Subject,
                cancellationToken
            );

            if (existingProvider is null)
            {
                var newProvider = new UserProvider
                {
                    Provider = GoogleProviderName,
                    ProviderUserId = payload.Subject,
                    User = user
                };

                await _userProviderRepository.AddAsync(newProvider, cancellationToken);
                isNewProvider = true;
                existingProvider = newProvider;
            }

            if (isNewUser|| isNewProvider)
                await _userRepository.SaveChangesAsync(cancellationToken);

            var token = _tokenService.GenerateToken(user, existingProvider.Provider);
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresMinutes);

            return new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Provider = existingProvider.Provider,
                ProviderUserId = existingProvider.ProviderUserId,
                IsNewUser = isNewUser,
                IsNewProvider = isNewProvider,
                AccessToken = token,
                ExpiresAtUtc = expiresAtUtc
            };
        }
    }
}
