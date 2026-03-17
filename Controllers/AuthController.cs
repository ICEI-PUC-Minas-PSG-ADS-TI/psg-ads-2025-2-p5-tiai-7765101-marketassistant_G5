using MarketAPI.Contracts.Auth;
using MarketAPI.Repositories.Interfaces;
using MarketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleTokenValidator _googleTokenValidator;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(
            IAuthService authService,
            IGoogleTokenValidator googleTokenValidator,
            IConfiguration configuration,
            IUserRepository userRepository
        )
        {
            _authService = authService;
            _googleTokenValidator = googleTokenValidator;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost("google")]
        public async Task<ActionResult<AuthResponse>> GoogleLogin(
            [FromHeader(Name = "Invitation-Code")] string? invitationCode,
            [FromBody] GoogleLoginRequest request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(request.IdToken))
                return BadRequest("IdToken is required.");

            var payload = await _googleTokenValidator.ValidateIdTokenAsync(request.IdToken, cancellationToken);

            if (payload is null)
                return Unauthorized("Invalid Google token.");
            
            var invitationCodeFromConfig = _configuration["Invitation:Code"]?.Trim();
            AuthResponse? result = null;

            if (invitationCode == invitationCodeFromConfig)
            {
                result = await _authService.LoginWithGoogleAsync(payload, cancellationToken);

                if (result is null)
                    return Unauthorized();

                else
                {
                    if (string.IsNullOrWhiteSpace(invitationCodeFromConfig))
                        return Unauthorized("Invitation code is not configured.");

                    if (string.IsNullOrWhiteSpace(invitationCode) ||
                        !string.Equals(invitationCode.Trim(), invitationCodeFromConfig, StringComparison.Ordinal))
                    {
                        return Unauthorized("Invalid invitation code.");
                    }
                }       
            } else if (invitationCode is null)
            {
                var userAlreadyExists = _userRepository.GetByEmailAsync(payload.Email, cancellationToken).GetAwaiter().GetResult();

                if (userAlreadyExists is not null)
                    result = await _authService.LoginWithGoogleAsync(payload, cancellationToken);


                if (result is null)
                    return Unauthorized();
            }

            return Ok(result);
        }
    }
}
