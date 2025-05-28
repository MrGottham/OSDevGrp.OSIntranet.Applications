using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class IsAccountingViewerWithoutAccountingNumberTests : UserHelperTestBase
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
    [TestCase(true)]
    [TestCase(false)]
    public void IsAccountingViewer_WhenUserHasAccountingClaimAndAccountingViewerClaim_ReturnsTrue(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingViewerClaim: true, hasAccountingViewerClaimValue: withValue);
        bool result = sut.IsAccountingViewer(user);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingViewer_WhenUserHasAccountingClaimButNoAccountingViewerClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingViewerClaim: false);
        bool result = sut.IsAccountingViewer(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public void IsAccountingViewer_WhenUserDoesNotHaveccountingClaimButAccountingViewerClaim_ReturnsFalse(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingViewerClaim: true, hasAccountingViewerClaimValue: withValue);
        bool result = sut.IsAccountingViewer(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingViewer_WhenUserDoesNotHaveccountingClaimAndNoAccountingViewerClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingViewerClaim: false);
        bool result = sut.IsAccountingViewer(user);

        Assert.That(result, Is.False);
    }

    private IPermissionChecker CreateSut() => CreatePermissionChecker();
}