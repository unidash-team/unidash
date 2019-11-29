using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
using System.Transactions;
using Foodies.Foody.Core.Security;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Foodies.Foody.Core.Tests
{
    public class JwtServiceGeneratorTests
    {
        [Fact]
        public void Create_NewToken_CanGenerate()
        {
            // Arrange
            var service = CreateDefaultTokenService();
            var userId = "12345";

            // Act
            string jwt = service.WriteToken(new Dictionary<string, string> {{ "uid", userId }}, 
                meta: new JwtTokenMeta {
                    ExpiresAt = DateTime.Now.AddDays(1),
                    Audience = "foody",
                    Issuer = "foody"
                });

            // Assert
            Assert.NotEmpty(jwt);
        }

        [Fact]
        public void Validate_Token_Successfully()
        {
            // Arrange
            var services = CreateDefaultTokenService();
            var userId = "12345";
            var audience = "foody";
            var issuer = "foody";

            // Act
            string jwt = services.WriteToken(new Dictionary<string, string> {{"uid", userId}},
                new JwtTokenMeta
                {
                    ExpiresAt = DateTime.Now.AddDays(1),
                    Audience = audience,
                    Issuer = issuer
                });

            // Assert
            var isValid = services.ValidateToken(jwt, new TokenValidationParameters 
            {
                ValidAudience = audience,
                ValidIssuer = issuer
            });

            Assert.True(isValid);
        }

        [Fact]
        public void Validate_TokenWithWrongParameters_Fail()
        {
            // Arrange
            var services = CreateDefaultTokenService();
            const string userId = "12345";
            const string audience = "foody";
            const string issuer = "foody";
            const string validateAudience = "dhbw";
            const string validateIssuer = issuer;

            // Act
            var jwt = services.WriteToken(new Dictionary<string, string> { { "uid", userId } },
                new JwtTokenMeta
                {
                    ExpiresAt = DateTime.Now.AddDays(1),
                    Audience = audience,
                    Issuer = issuer
                });

            // Assert
            var isValid = services.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidAudience = validateAudience, ValidIssuer = validateIssuer
            });

            Assert.False(isValid);
        }

        [Fact]
        public void ReadNameClaim_Token_Successfully()
        {
            // Arrange
            var services = CreateDefaultTokenService();
            const string userId = "12345";
            const string audience = "foody";
            const string issuer = "foody";

            // Act
            var jwt = services.WriteToken(new Dictionary<string, string> {{ ClaimTypes.Name, userId }},
                new JwtTokenMeta
                {
                    ExpiresAt = DateTime.Now.AddDays(1),
                    Audience = audience,
                    Issuer = issuer
                });

            var claims = services.ReadToken(jwt, new TokenValidationParameters
            {
                ValidAudience = audience, ValidIssuer = issuer
            });

            // Assert
            Assert.Equal(userId, claims.Identity.Name);
        }

        // Seed work
        private JwtTokenService CreateDefaultTokenService()
        {
            var secret = "verysecuresecretindeed!123";
            return new JwtTokenService(signingKey: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)));
        }
    }
}
