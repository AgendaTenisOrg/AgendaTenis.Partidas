using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AgendaTenis.Partidas.WebApi.ConfiguracaoDeServicos;

public static class AutenticacaoDI
{
    public static void AdicionarAutenticacaoJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection(nameof(JwtOptions))["ChaveSecreta"]));

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,

            RequireExpirationTime = true,
            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
        });
    }

    public class JwtOptions
    {
        public SigningCredentials SigningCredentials { get; set; }
    }
}
