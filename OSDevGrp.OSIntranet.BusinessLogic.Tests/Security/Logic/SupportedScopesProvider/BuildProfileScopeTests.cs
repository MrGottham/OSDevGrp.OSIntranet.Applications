using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SupportedScopesProvider
{
    [TestFixture]
    public class BuildProfileScopeTests
    {
        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereNameIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereNameIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereNameIsEqualToProfileScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.Name, Is.EqualTo(ScopeHelper.ProfileScope));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.Description, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.Description, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereDescriptionIsEqualToDescriptionForProfileScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.Description, Is.EqualTo("this scope requests access to the End-User's profile."));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.RelatedClaims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.RelatedClaims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsThreeRelatedClaim()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.RelatedClaims.Count(), Is.EqualTo(3));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimTypes.Name)]
        [TestCase(ClaimTypes.GivenName)]
        [TestCase(ClaimTypes.Surname)]
        public void BuildProfileScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsRelatedClaim(string expectedRelatedClaim)
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildProfileScope();

            Assert.That(result.RelatedClaims.Contains(expectedRelatedClaim), Is.True);
        }
    }
}