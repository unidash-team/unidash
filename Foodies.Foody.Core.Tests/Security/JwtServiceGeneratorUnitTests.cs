using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Foodies.Foody.Core.Security;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Foodies.Foody.Core.Tests.Security
{
    public class JwtServiceGeneratorUnitTests
    {
        [Fact]
        public void Create_NewToken_CanGenerate()
        {
            // Arrange
            var service = CreateDefaultTokenService();
            const string userId = "12345";

            // Act
            var jwt = service.WriteToken(new Dictionary<string, string> {{ "uid", userId }}, 
                meta: new JwtTokenMeta {
                    ExpiresAt = DateTime.Now.AddDays(1),
                    Audience = "foody",
                    Issuer = "foody"
                });

            // Assert
            jwt.Should().NotBeEmpty();
        }

        [Fact]
        public void Validate_Token_Successfully()
        {
            // Arrange
            var services = CreateDefaultTokenService();
            const string userId = "12345";
            const string audience = "foody";
            const string issuer = "foody";

            // Act
            var jwt = services.WriteToken(new Dictionary<string, string> {{"uid", userId}},
                new JwtTokenMeta
                {
                    ExpiresAt = DateTime.Now.AddDays(1),
                    Audience = audience,
                    Issuer = issuer
                });

            var isValid = services.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidAudience = audience,
                ValidIssuer = issuer
            });

            // Assert
            isValid.Should().BeTrue();
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

            var isValid = services.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidAudience = validateAudience,
                ValidIssuer = validateIssuer
            });

            // Assert
            isValid.Should().BeFalse();
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
            claims.Identity.Name
                .Should()
                .Be(userId);
        }

        // Seed work
        private JwtTokenService CreateDefaultTokenService()
        {
            const string secret = "verysecuresecretindeed!123";
            return new JwtTokenService(signingKey: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)));
        }
    }
}
