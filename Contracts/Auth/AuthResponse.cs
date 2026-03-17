namespace MarketAPI.Contracts.Auth
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string ProviderUserId { get; set; } = string.Empty;
        public bool IsNewUser { get; set; }
        public bool IsNewProvider { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
