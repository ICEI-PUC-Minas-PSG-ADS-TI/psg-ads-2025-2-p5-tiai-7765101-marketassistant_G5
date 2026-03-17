using MarketAPI.Entities;

namespace MarketAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, string provider);
    }
}
