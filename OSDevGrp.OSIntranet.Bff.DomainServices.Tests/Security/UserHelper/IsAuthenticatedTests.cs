using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class IsAuthenticatedTests : UserHelperTestBase
{
    #region Prrivate variables

    private Mock<IPermissionValidator>? _permissionValidatorMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionValidatorMock = new Mock<IPermissionValidator>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void IsAuthenticated_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionValidatorWithGivenClaimsPrincipal()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        sut.IsAuthenticated(user);

        _permissionValidatorMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public void IsAuthenticated_WhenCalled_ReturnsIsAuthenticatedFromPermissionValidator(bool isAuthenticated)
    {
        IPermissionChecker sut = CreateSut(isAuthenticated: isAuthenticated);

        ClaimsPrincipal user = isAuthenticated
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        bool result = sut.IsAuthenticated(user);

        Assert.That(result, Is.EqualTo(isAuthenticated));
    }

    private IPermissionChecker CreateSut(bool? isAuthenticated = null)
    {
        _permissionValidatorMock!.Setup(m => m.IsAuthenticated(It.IsAny<ClaimsPrincipal>()))
            .Returns(isAuthenticated ?? _fixture!.Create<bool>());

        return CreatePermissionChecker(_permissionValidatorMock!.Object);
    }
}