using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MarketAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [Authorize]
        [HttpGet("me")]
        public ActionResult<object> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            var email = User.FindFirstValue(ClaimTypes.Email)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Email);

            var name = User.FindFirstValue(ClaimTypes.Name)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Name);

            var provider = User.FindFirstValue("provider");

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Name = name,
                Provider = provider
            });
        }
    }
}
