using FluentAssertions;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class PasswordServiceTest
    {
        private readonly IPasswordService _passwordService;

        public PasswordServiceTest()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void Should_Hash_And_Successfully_Verify_Password()
        {
            const string password = "Password1234";

            var hashedPassword = _passwordService.HashPassword(password);
            var verificationResult = _passwordService.VerifyHashedPassword(hashedPassword, password);

            verificationResult.Should().BeTrue();
        }

        [Fact]
        public void Should_Hash_And_Unsuccessfully_Verify_Password()
        {
            const string password = "Password1234";
            const string wrongPassword = "Password";

            var hashedPassword = _passwordService.HashPassword(password);
            var verificationResult = _passwordService.VerifyHashedPassword(hashedPassword, wrongPassword);

            verificationResult.Should().BeFalse();
        }
    }
}