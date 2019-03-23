using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class GetClaimsTests : SecurityRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetClaimsAsync_WhenCalled_ReturnsClaims()
        {
            ISecurityRepository sut = CreateSut();

            IList<Claim> result = (await sut.GetClaimsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        [Category("IntegrationTest")]
        [TestCase(ClaimHelper.SecurityAdminClaimType, "")]
        [TestCase(ClaimHelper.AccountingClaimType, "")]
        public async Task GetClaimsAsync_WhenCalled_ContainsClaimType(string claimType, string claimValue)
        {
            ISecurityRepository sut = CreateSut();

            Claim result = (await sut.GetClaimsAsync()).SingleOrDefault(claim => string.Compare(claim.Type, claimType, StringComparison.Ordinal) == 0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Type, Is.EqualTo(claimType));
            Assert.That(result.Value, Is.EqualTo(claimValue));
        }
    }
}
