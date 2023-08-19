using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NorthWind.Sales.BusinessObjects.Interfaces.Services;
using NorthWind.Token.Services.Cryptography;
using NorthWind.Token.Services.JWT;
using System.Security.Cryptography;

namespace NorthWind.Token.Services
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddTokenServices(
            this IServiceCollection services,
            IConfiguration configuration, string jwtSettingsName)
        {
            services.AddSingleton<IAuthTokenService, JwtService>();
            services.AddSingleton<ICryptographyService, CryptographyService>();
            services.Configure<JwtSettings>(configuration.GetSection(jwtSettingsName));

            var jwtSettings = configuration.GetSection(jwtSettingsName).Get<JwtSettings>();


            services.AddSingleton(provider =>
            {
                var rsa = RSA.Create();
                //// Para BEGIN RSA PRIVATE KEY
                //rsa.ImportRSAPrivateKey(Convert.FromBase64String(jwtSettings.AccessTokenSettings.PrivateKey).Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", ""), out int _);
                //// Para BEGIN PRIVATE KEY
                //rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(jwtSettings.AccessTokenSettings.PrivateKey.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "")), out int _);

                // Para las dos etiquetas PEM anteriores
                rsa.ImportFromPem(jwtSettings.AccessTokenSettings.PrivateKey);
                return new RsaSecurityKey(rsa);
            });

            return services;
        }


    }
}
