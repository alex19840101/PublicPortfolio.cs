using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ServiceCollectionsExtensions
{
    public static class TokenValidationParametersHelper
    {
        /// <summary>
        /// Получение TokenValidationParameters-настроек для JWT-аутентификации и авторизации
        /// </summary>
        /// <param name="configuration"> IConfiguration-конфигурация ((ConfigurationManager)) </param>
        /// <returns> TokenValidationParameters </returns>
        public static TokenValidationParameters GetTokenValidationParametersForJWT(this IConfiguration configuration)
        {
            return new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(configuration["JWT:KEY"]!)),
                ValidateIssuerSigningKey = true
            };
        }

    }
}
