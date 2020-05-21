using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Unidash.Core.Security;
using Xunit;

namespace Unidash.Core.Tests.Security
{
    public class PasswordTests
    {
        [Fact]
        public void Password_Hash_Correctly()
        {
            // Arrange
            const string rawPassword = "verysecurepassword!123";
            var service = CreateDefaultPasswordService();
            var salt = service.GenerateRandomSalt();

            // Act
            var hashedPassword = service.Hash(rawPassword, salt);
            var isValid = service.Validate(rawPassword, salt, hashedPassword);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Password_HashWithBytesAndBase64_Equal()
        {
            // Arrange
            var service = CreateDefaultPasswordService();
            const string rawPassword = "verysecurepassword!123";

            // Act
            var bytesSalt = service.GenerateRandomSalt();
            var stringSalt = service.GenerateRandomSaltAsBase64();

            var bytesHash = service.Hash(rawPassword, bytesSalt);
            var stringHash = service.Hash(rawPassword, stringSalt);

            var isBytesHashValid = service.Validate(rawPassword, bytesSalt, bytesHash);
            var isStringHashValid = service.Validate(rawPassword, stringSalt, stringHash);

            // Assert
            isBytesHashValid.Should().BeTrue();
            isStringHashValid.Should().BeTrue();
            isBytesHashValid.Should().Be(isStringHashValid);
        }

        private PasswordService CreateDefaultPasswordService() => new PasswordService();
    }
}
