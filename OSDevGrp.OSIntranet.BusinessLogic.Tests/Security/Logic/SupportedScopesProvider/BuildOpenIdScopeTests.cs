using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SupportedScopesProvider
{
    [TestFixture]
    public class BuildOpenIdScopeTests
    {
        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereNameIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereNameIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereNameIsEqualToOpenIdScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.Name, Is.EqualTo(ScopeHelper.OpenIdScope));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.Description, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.Description, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereDescriptionIsEqualToDescriptionForOpenIdScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.Description, Is.EqualTo("this mandatory scope indicates that the application intends to use OpenID Connect to verify the user's identity."));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.RelatedClaims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.RelatedClaims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsOneRelatedClaim()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.RelatedClaims.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimTypes.NameIdentifier)]
        public void BuildOpenIdScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsRelatedClaim(string expectedRelatedClaim)
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildOpenIdScope();

            Assert.That(result.RelatedClaims.Contains(expectedRelatedClaim), Is.True);
        }
    }
}