using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class IsAccountingViewerWithAccountingNumberTests : UserHelperTestBase
{
    #region Private variables

    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1, "1")]
    [TestCase(2, "1,2")]
    [TestCase(3, "1,2,3")]
    [TestCase(4, "1,2,3,*")]
    [TestCase(5, "*")]
    public void IsAccountingViewer_WhenUserHasAccountingClaimAndAccountingViewerClaimWithValueAllowingAccountingNumber_ReturnsTrue(int accountingNumber, string accountingViewerClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingViewerClaim: true, hasAccountingViewerClaimValue: true, accountingViewerClaimValue: accountingViewerClaimValue);
        bool result = sut.IsAccountingViewer(user, accountingNumber);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1, "2,3,4,5")]
    [TestCase(2, "1,3,4,5")]
    [TestCase(3, "1,2,4,5")]
    [TestCase(4, "x,y")]
    [TestCase(5, "")]
    public void IsAccountingViewer_WhenUserHasAccountingClaimAndAccountingViewerClaimWithValueNotAllowingAccountingNumber_ReturnsFalse(int accountingNumber, string accountingViewerClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingViewerClaim: true, hasAccountingViewerClaimValue: true, accountingViewerClaimValue: accountingViewerClaimValue);
        bool result = sut.IsAccountingViewer(user, accountingNumber);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingViewer_WhenUserHasAccountingClaimButNoAccountingViewerClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingViewerClaim: false);
        bool result = sut.IsAccountingViewer(user, _fixture.Create<int>());

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1, "1")]
    [TestCase(2, "1,2")]
    [TestCase(3, "1,2,3")]
    [TestCase(4, "1,2,3,*")]
    [TestCase(5, "*")]
    public void IsAccountingViewer_WhenUserDoesNotHaveccountingClaimButAccountingViewerClaimWithValueAllowingAccountingNumber_ReturnsFalse(int accountingNumber, string accountingViewerClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingViewerClaim: true, hasAccountingViewerClaimValue: true, accountingViewerClaimValue: accountingViewerClaimValue);
        bool result = sut.IsAccountingViewer(user, accountingNumber);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1, "2,3,4,5")]
    [TestCase(2, "1,3,4,5")]
    [TestCase(3, "1,2,4,5")]
    [TestCase(4, "x,y")]
    [TestCase(5, "")]
    public void IsAccountingViewer_WhenUserDoesNotHaveccountingClaimButAccountingViewerClaimWithValueNotAllowingAccountingNumber_ReturnsFalse(int accountingNumber, string accountingViewerClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingViewerClaim: true, hasAccountingViewerClaimValue: true, accountingViewerClaimValue: accountingViewerClaimValue);
        bool result = sut.IsAccountingViewer(user, accountingNumber);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingViewer_WhenUserDoesNotHaveccountingClaimAndNoAccountingViewerClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingViewerClaim: false);
        bool result = sut.IsAccountingViewer(user, _fixture.Create<int>());

        Assert.That(result, Is.False);
    }

    private IPermissionChecker CreateSut() => CreatePermissionChecker();
}