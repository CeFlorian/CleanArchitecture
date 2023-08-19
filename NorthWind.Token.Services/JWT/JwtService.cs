using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NorthWind.Sales.BusinessObjects.Interfaces.Services;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NorthWind.Token.Services.JWT
{
    public class JwtService : IAuthTokenService
    {
        private readonly IOptions<JwtSettings> Settings;
        private readonly RsaSecurityKey RsaSecurityKey;

        public JwtService(IOptions<JwtSettings> settings, RsaSecurityKey rsaSecurityKey)
        {
            Settings = settings;
            RsaSecurityKey = rsaSecurityKey;
        }

        public Task<string> GenerateAccessToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                key: RsaSecurityKey,
                algorithm: SecurityAlgorithms.RsaSha256
            );

            var claimsIdentity = new ClaimsIdentity();

            // Token de acceso solo con el ID del usuario
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            // Claim de scope que se utilizara para autorizacion
            var scope = user.Claims?.SingleOrDefault(c => c.Type == "scope");
            if (scope != null) claimsIdentity.AddClaim(new System.Security.Claims.Claim("scope", string.Join(" ", scope.Value)));

            var jwtHandler = new JwtSecurityTokenHandler();

            var jwt = jwtHandler.CreateJwtSecurityToken(
                issuer: Settings.Value.AccessTokenSettings.Issuer,
                audience: Settings.Value.AccessTokenSettings.Audience,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(Settings.Value.AccessTokenSettings.LifeTimeInSeconds),
                issuedAt: DateTime.UtcNow,
                signingCredentials: signingCredentials);

            var serializedJwt = jwtHandler.WriteToken(jwt);

            return Task.FromResult(serializedJwt);
        }

        public Task<string> GenerateIdToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                key: RsaSecurityKey,
                algorithm: SecurityAlgorithms.RsaSha256
            );

            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name, user.Name));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Email, user.Email));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.GivenName, user.Name));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Surname, user.LastName));

            // Si el usuario tiene claims guardados se agregan
            if (user.Claims != null && user.Claims.Count > 0)
            {
                foreach (var c in user.Claims)
                {
                    claimsIdentity.AddClaim(new System.Security.Claims.Claim(c.Type, c.Value));
                }
            }

            var jwtHandler = new JwtSecurityTokenHandler();

            var jwt = jwtHandler.CreateJwtSecurityToken(
                issuer: Settings.Value.AccessTokenSettings.Issuer,
                audience: Settings.Value.AccessTokenSettings.Audience,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(Settings.Value.AccessTokenSettings.LifeTimeInSeconds),
                issuedAt: DateTime.UtcNow,
                signingCredentials: signingCredentials);

            var serializedJwt = jwtHandler.WriteToken(jwt);

            return Task.FromResult(serializedJwt);
        }

        public Task<string> GenerateRefreshToken()
        {
            var size = Settings.Value.RefreshTokenSettings.Length;
            var buffer = new byte[size];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return Task.FromResult(Convert.ToBase64String(buffer));
        }

        public Task<int> GetRefreshTokenLifetimeInMinutes()
        {
            return Task.FromResult(Settings.Value.RefreshTokenSettings.LifeTimeInMinutes);
        }

        public Task<Guid> GetUserIdFromToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Se desactiva porque es posible que debamos validar un token caducado
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Settings.Value.AccessTokenSettings.Issuer,
                    ValidAudience = Settings.Value.AccessTokenSettings.Audience,
                    IssuerSigningKey = RsaSecurityKey,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };

                var jwtHandler = new JwtSecurityTokenHandler();
                var claims = jwtHandler.ValidateToken(token, tokenValidationParameters, out _);
                var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

                return Task.FromResult(userId);
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException(ex.Message, ex);
            }
        }

        public Task<bool> IsTokenValid(string token, bool validateLifeTime)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = validateLifeTime,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Settings.Value.AccessTokenSettings.Issuer,
                ValidAudience = Settings.Value.AccessTokenSettings.Audience,
                IssuerSigningKey = RsaSecurityKey,
                ClockSkew = TimeSpan.FromMinutes(0)
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            try
            {
                jwtHandler.ValidateToken(token, tokenValidationParameters, out _);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
