using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hotelio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Hotelio.Auth
{
    public interface ITokenManager
    {
        Task<string> CreateAccessTokenAsync(User user);
    }

    public class TokenManager : ITokenManager
    {
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly UserManager<User> _userManager;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenManager(IConfiguration configuration, UserManager<User> userManager)
        {
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _userManager = userManager;
            _issuer = configuration["JWT:ValidIssuer"];
            _audience = configuration["JWT:ValidAudience"];
        }

        public async Task<string> CreateAccessTokenAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id)
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var accessSecurityToken = new JwtSecurityToken
            (
                expires: DateTime.UtcNow.AddHours(1),
                issuer: _issuer,
                audience: _audience,
                claims: authClaims,
                signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
        }
    }
}
