using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class IsAccountingAdministratorTests : UserHelperTestBase
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
    public void IsAccountingAdministrator_WhenUserHasAccountingClaimAndAccountingAdministratorClaim_ReturnsTrue(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingAdministratorClaim: true, hasAccountingAdministratorClaimValue: withValue);
        bool result = sut.IsAccountingAdministrator(user);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingAdministrator_WhenUserHasAccountingClaimButNoAccountingAdministratorClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingAdministratorClaim: false);
        bool result = sut.IsAccountingAdministrator(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public void IsAccountingAdministrator_WhenUserDoesNotHaveccountingClaimButAccountingAdministratorClaim_ReturnsFalse(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingAdministratorClaim: true, hasAccountingAdministratorClaimValue: withValue);
        bool result = sut.IsAccountingAdministrator(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingAdministrator_WhenUserDoesNotHaveccountingClaimAndNoAccountingAdministratorClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingAdministratorClaim: false);
        bool result = sut.IsAccountingAdministrator(user);

        Assert.That(result, Is.False);
    }

    private IPermissionChecker CreateSut() => CreatePermissionChecker();
}