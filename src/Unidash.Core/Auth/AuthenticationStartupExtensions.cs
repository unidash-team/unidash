using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Unidash.Core.Options;
using Unidash.Core.Security;

namespace Unidash.Core.Auth
{
    public static class AuthenticationStartupExtensions
    {
        /// <summary>
        /// <see cref="AddUnidashAuthentication(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Configuration.IConfigurationSection)"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        public static void AddUnidashAuthentication(this IServiceCollection services, IConfigurationSection configurationSection) =>
            AddUnidashAuthentication(services, configurationSection.Bind);

        /// <summary>
        /// Adds the default authentication method used for Unidash and additional services for JWT, etc.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsBuilder"></param>
        public static void AddUnidashAuthentication(this IServiceCollection services, Action<UnidashAuthenticationOptions> optionsBuilder)
        {
            UnidashAuthenticationOptions options = new UnidashAuthenticationOptions();
            optionsBuilder.Invoke(options);

            var unidashSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecurityKey));

            services.AddAuthentication(configure =>
                {
                    configure.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    configure.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = "unidash",
                        ValidateAudience = true,

                        ValidIssuer = "unidash",
                        ValidateIssuer = true,

                        IssuerSigningKey = unidashSecurityKey,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };

                    jwtOptions.IncludeErrorDetails = true;
                    jwtOptions.RequireHttpsMetadata = false;
                    jwtOptions.SaveToken = true;
                })
                .AddCookie();

            services.AddSingleton<JwtTokenService>(provider => new JwtTokenService(unidashSecurityKey));
        }
    }
}
