using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Unidash.Core.Auth
{
    public static class AuthenticationStartupExtensions
    {
        public static void AddUnidashAuthentication(this IServiceCollection collection)
        {
            collection.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "auth.unidash.net",
                        ValidateIssuer = true,

                        ValidateAudience = false,

                        ValidateIssuerSigningKey = false
                    };
                });
        }
    }
}
