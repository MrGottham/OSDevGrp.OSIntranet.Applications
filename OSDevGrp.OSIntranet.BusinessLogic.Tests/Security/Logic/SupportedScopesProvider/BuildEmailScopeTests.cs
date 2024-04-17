using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SupportedScopesProvider
{
    [TestFixture]
    public class BuildEmailScopeTests
    {
        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereNameIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereNameIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereNameIsEqualToEmailScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.Name, Is.EqualTo(ScopeHelper.EmailScope));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.Description, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.Description, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereDescriptionIsEqualToDescriptionForEmailScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.Description, Is.EqualTo("this scope requests access to the End-User's mail addresses."));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.RelatedClaims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.RelatedClaims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsOneRelatedClaim()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.RelatedClaims.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimTypes.Email)]
        public void BuildEmailScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsRelatedClaim(string expectedRelatedClaim)
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildEmailScope();

            Assert.That(result.RelatedClaims.Contains(expectedRelatedClaim), Is.True);
        }
    }
}