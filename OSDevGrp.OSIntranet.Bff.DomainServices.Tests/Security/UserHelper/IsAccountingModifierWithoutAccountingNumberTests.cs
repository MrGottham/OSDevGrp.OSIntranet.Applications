using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class IsAccountingModifierWithoutAccountingNumberTests : UserHelperTestBase
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
    public void IsAccountingModifier_WhenUserHasAccountingClaimAndAccountingModifierClaim_ReturnsTrue(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingModifierClaim: true, hasAccountingModifierClaimValue: withValue);
        bool result = sut.IsAccountingModifier(user);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingModifier_WhenUserHasAccountingClaimButNoAccountingModifierClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingModifierClaim: false);
        bool result = sut.IsAccountingModifier(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public void IsAccountingModifier_WhenUserDoesNotHaveccountingClaimButAccountingModifierClaim_ReturnsFalse(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingModifierClaim: true, hasAccountingModifierClaimValue: withValue);
        bool result = sut.IsAccountingModifier(user);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingModifier_WhenUserDoesNotHaveccountingClaimAndNoAccountingModifierClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingModifierClaim: false);
        bool result = sut.IsAccountingModifier(user);

        Assert.That(result, Is.False);
    }

    private IPermissionChecker CreateSut() => CreatePermissionChecker();
}