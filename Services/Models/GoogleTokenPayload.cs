namespace MarketAPI.Services.Models
{
    public class GoogleTokenPayload
    {
        public string Subject { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
