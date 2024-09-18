using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SupportedScopesProvider
{
    [TestFixture]
    public class BuildWebApiScopeTests
    {
        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereNameIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereNameIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereNameIsEqualToWebApiScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.Name, Is.EqualTo(ScopeHelper.WebApiScope));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.Description, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereDescriptionIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.Description, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereDescriptionIsEqualToDescriptionForWebApiScope()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.Description, Is.EqualTo("this scope requests access to the OS Development Group Web API."));
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotNull()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.RelatedClaims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereRelatedClaimsIsNotEmpty()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.RelatedClaims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsFifteenRelatedClaim()
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.RelatedClaims.Count(), Is.EqualTo(15));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimHelper.ExternalUserIdentifierClaimType)]
        [TestCase(ClaimHelper.FriendlyNameClaimType)]
        [TestCase(ClaimHelper.SecurityAdminClaimType)]
        [TestCase(ClaimHelper.AccountingClaimType)]
        [TestCase(ClaimHelper.AccountingAdministratorClaimType)]
        [TestCase(ClaimHelper.AccountingCreatorClaimType)]
        [TestCase(ClaimHelper.AccountingModifierClaimType)]
        [TestCase(ClaimHelper.AccountingViewerClaimType)]
        [TestCase(ClaimHelper.MediaLibraryClaimType)]
        [TestCase(ClaimHelper.MediaLibraryModifierClaimType)]
        [TestCase(ClaimHelper.MediaLibraryLenderClaimType)]
        [TestCase(ClaimHelper.CommonDataClaimType)]
        [TestCase(ClaimHelper.ContactsClaimType)]
        [TestCase(ClaimHelper.CountryCodeClaimType)]
        [TestCase(ClaimHelper.CollectNewsClaimType)]
        public void BuildWebApiScope_WhenCalled_ReturnsScopeWhereRelatedClaimsContainsRelatedClaim(string expectedRelatedClaim)
        {
            IScope result = BusinessLogic.Security.Logic.SupportedScopesProvider.BuildWebApiScope();

            Assert.That(result.RelatedClaims.Contains(expectedRelatedClaim), Is.True);
        }
    }
}