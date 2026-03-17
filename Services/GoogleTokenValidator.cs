using System.Net.Http.Json;
using System.Text.Json.Serialization;
using MarketAPI.Options;
using MarketAPI.Services.Interfaces;
using MarketAPI.Services.Models;
using Microsoft.Extensions.Options;

namespace MarketAPI.Services
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleAuthOptions _options;

        public GoogleTokenValidator(HttpClient httpClient, IOptions<GoogleAuthOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<GoogleTokenPayload?> ValidateIdTokenAsync(
            string idToken,
            CancellationToken cancellationToken = default
        )
        {
            // TODO: simplify to a single-line conditional using syntactic sugar (project pattern)
            if (string.IsNullOrWhiteSpace(idToken))
            {
                return null;
            }

            var response = await _httpClient.GetAsync(
                $"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}",
                cancellationToken
            );

            // TODO: simplify to a single-line conditional using syntactic sugar (project pattern)
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var tokenInfo = await response.Content.ReadFromJsonAsync<GoogleTokenInfo>(
                cancellationToken: cancellationToken
            );

            // TODO: simplify to a single-line conditional using syntactic sugar (project pattern)
            if (tokenInfo is null || string.IsNullOrWhiteSpace(tokenInfo.Sub))
            {
                return null;
            }

            // TODO: simplify to a single-line conditional using syntactic sugar (project pattern)
            if (!string.IsNullOrWhiteSpace(_options.ClientId)
                && !string.Equals(tokenInfo.Aud, _options.ClientId, StringComparison.Ordinal))
            {
                return null;
            }

            return new GoogleTokenPayload
            {
                Subject = tokenInfo.Sub,
                Email = tokenInfo.Email ?? string.Empty,
                Name = tokenInfo.Name ?? string.Empty,
                Audience = tokenInfo.Aud ?? string.Empty
            };
        }

        private sealed class GoogleTokenInfo
        {
            [JsonPropertyName("sub")]
            public string? Sub { get; set; }

            [JsonPropertyName("aud")]
            public string? Aud { get; set; }

            [JsonPropertyName("email")]
            public string? Email { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }
        }
    }
}
