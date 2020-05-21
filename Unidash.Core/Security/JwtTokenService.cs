using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Unidash.Core.Security
{
    public class JwtTokenService
    {
        public SecurityKey SigningKey { get; }

        public JwtTokenService(SecurityKey signingKey)
        {
            SigningKey = signingKey;
        }

        public string WriteToken(IDictionary<string, string> claims, JwtTokenMeta meta)
        {
            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor()
            {
                Audience = meta.Audience,
                Issuer = meta.Issuer,
                Expires = meta.ExpiresAt,

                Subject = new ClaimsIdentity(claims
                    .Select(c => new Claim(c.Key, c.Value.ToString()))),

                SigningCredentials = new SigningCredentials(SigningKey, 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public bool ValidateToken(string jwt, TokenValidationParameters parameters)
        {
            var handler = new JwtSecurityTokenHandler();
            parameters.IssuerSigningKey = SigningKey;

            try
            {
                var claimsPrincipal = handler.ValidateToken(jwt, parameters, out SecurityToken token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ClaimsPrincipal ReadToken(string jwt, TokenValidationParameters parameters)
        {
            var handler = new JwtSecurityTokenHandler();
            parameters.IssuerSigningKey = SigningKey;

            return handler.ValidateToken(jwt, parameters, out SecurityToken token);
        }
    }
}