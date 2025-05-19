using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OffersHub.Application.Services.Users
{
    public static class TokenService
    {
        private static readonly string _secretKey = "usiyvarulodmzearsufevsciskabadonze";  // Use a strong secret key
        private static readonly string _issuer = "OffersHub";  // Set the issuer (can be your app name)
        private static readonly string _audience = "OffersHub";  // Set the audience (can be your app name)

        public static string GenerateToken(string username, string role)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  // JWT unique identifier
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),  // Set token expiration (1 hour)
                signingCredentials: creds
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtSecurityTokenHandler().WriteToken(token);  // Return the token as a string
        }

        public static string GenerateSecurityToken(string userName, string role, IOptions<JWTConfig> options)
        {
            if (string.IsNullOrEmpty(options.Value.Secret))
            {
                throw new ArgumentNullException(nameof(options.Value.Secret), "JWT Secret is required but was not provided.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(options.Value.Secret!);

            //for debug
            //var test = options.Value.Secret;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, role)
                }),

                Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpirationInMinutes),
                Audience = "localhost",
                Issuer = "localhost",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            try { 
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating token: {ex.Message}");
                throw;
            }
        }
    }
    public class JWTConfig
    {
        public string? Secret { get; set; }
        public int ExpirationInMinutes { get; set; }
    }

}
