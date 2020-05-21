using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Unidash.Core.Security;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Unidash.Core.Tests.Security
{
    public class JwtGeneratorTests
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
                    Audience = "unidash",
                    Issuer = "unidash"
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
            const string audience = "unidash";
            const string issuer = "unidash";

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
            const string audience = "unidash";
            const string issuer = "unidash";
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
            const string audience = "unidash";
            const string issuer = "unidash";

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
