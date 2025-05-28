using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class IsAccountingCreatorTests : UserHelperTestBase
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
    public void IsAccountingCreator_WhenUserHasAccountingClaimAndAccountingCreatorClaim_ReturnsTrue(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingCreatorClaim: true, hasAccountingCreatorClaimValue: withValue);
        bool result = sut.IsAccountingCreator(user);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingCreator_WhenUserHasAccountingClaimButNoAccountingCreatorClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingCreatorClaim: false);
        bool result = sut.IsAccountingCreator(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public void IsAccountingCreator_WhenUserDoesNotHaveccountingClaimButAccountingCreatorClaim_ReturnsFalse(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingCreatorClaim: true, hasAccountingCreatorClaimValue: withValue);
        bool result = sut.IsAccountingCreator(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingCreator_WhenUserDoesNotHaveccountingClaimAndNoAccountingCreatorClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingCreatorClaim: false);
        bool result = sut.IsAccountingCreator(user);

        Assert.That(result, Is.False);
    }

    private IPermissionChecker CreateSut() => CreatePermissionChecker();
}