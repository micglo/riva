using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using Moq;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class AccountClaimsCreatorServiceTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly AccountClaimsCreatorService _service;

        public AccountClaimsCreatorServiceTest()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _service = new AccountClaimsCreatorService(_roleRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAccountClaimsAsync_Should_Create_Account_Claims()
        {
            var roleId = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { roleId })
                .Build();
            var roles = new List<Role>
            {
                new Role(roleId, Array.Empty<byte>(), DefaultRoleEnumeration.User.DisplayName)
            };
            var roleJwtClaims = roles.Select(x => new Claim(JwtClaimTypes.Role, x.Name));
            var roleSecurityClaims = roles.Select(x => new Claim(ClaimTypes.Role, x.Name));
            var roleClaims = new List<Claim>(roleJwtClaims);
            roleClaims.AddRange(roleSecurityClaims);
            var expectedClaims = new List<Claim>(roleClaims)
            {
                new Claim(JwtClaimTypes.Subject, account.Id.ToString()),
                new Claim(JwtClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(JwtClaimTypes.EmailVerified, account.Confirmed.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Scope, ApiResourcesConstants.RivaIdentityApiResource.Name),
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };

            _roleRepositoryMock.Setup(x => x.FindByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(roles);

            var result = await _service.CreateAccountClaimsAsync(account);

            result.Should().BeEquivalentTo(expectedClaims);
        }

        [Fact]
        public async Task CreateAccountClaimsAsync_Should_Throw_ArgumentNullException_When_Account_Has_No_Roles()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Func<Task<List<Claim>>> result = async () => await _service.CreateAccountClaimsAsync(account);

            await result.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}