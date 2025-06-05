using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoModel;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Security.UserInfo.UserInfoFeature;

[TestFixture]
public class ExecuteAsyncTests : UserInfoFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IUserInfoProvider>? _userInfoProviderMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _userInfoProviderMock = new Mock<IUserInfoProvider>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertUserWasCalledOnSecurityContextFromGivenUserInfoRequest()
    {
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut();

        Mock<ISecurityContext> securityContextMock = CreateSecurityContextMock(_fixture!);
        UserInfoRequest request = CreateUserInfoRequest(_fixture!, securityContextMock.Object);
        await sut.ExecuteAsync(request);

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetUserInfoAsyncWasCalledOnUserInfoProviderWithUserFromGivenUserInfoRequest()
    {
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut();

        ClaimsPrincipal user = CreateAuthenticatedUser(_fixture!); 
        ISecurityContext securityContext = CreateSecurityContext(_fixture!, user: user);
        UserInfoRequest request = CreateUserInfoRequest(_fixture!, securityContext);
        await sut.ExecuteAsync(request);

        _userInfoProviderMock!.Verify(m => m.GetUserInfoAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetUserInfoAsyncWasCalledOnUserInfoProviderWithGivenCancellationToken()
    {
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut();

        UserInfoRequest request = CreateUserInfoRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _userInfoProviderMock!.Verify(m => m.GetUserInfoAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserInfoWasReturnedFromUserInfoProvider_ReturnsUserInfoResponseWhereUserInfoIsEqualToUserInfoResolvedByUserInfoProvider()
    {
        IUserInfoModel userInfoModel = _fixture!.CreateUserInfoModel(_random!);
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut(userInfoModel: userInfoModel);

        UserInfoRequest request = CreateUserInfoRequest(_fixture!);
        UserInfoResponse result = await sut.ExecuteAsync(request);

        Assert.That(result.UserInfo, Is.EqualTo(userInfoModel));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.MailAddress)]
    [TestCase(StaticTextKey.Permissions)]
    [TestCase(StaticTextKey.FinancialManagement)]
    [TestCase(StaticTextKey.Administrator)]
    [TestCase(StaticTextKey.Creator)]
    [TestCase(StaticTextKey.Modifier)]
    [TestCase(StaticTextKey.Viewer)]
    [TestCase(StaticTextKey.CommonData)]
    [TestCase(StaticTextKey.PrimaryAccounting)]
    [TestCase(StaticTextKey.Accountings)]
    public async Task ExecuteAsync_WhenUserInfoWasReturnedFromUserInfoProvider_ReturnsUserInfoResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut();

        UserInfoRequest request = CreateUserInfoRequest(_fixture!);
        UserInfoResponse result = await sut.ExecuteAsync(request);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserInfoWasNotReturnedFromUserInfoProvider_ThrowsInvalidOperationException()
    {
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut(hasUserInfo: false);

        UserInfoRequest request = CreateUserInfoRequest(_fixture!);
        try
        {
            await sut.ExecuteAsync(request);

            Assert.Fail($"An {nameof(InvalidOperationException)} should have been thrown, but no exception was thrown.");
        }
        catch (InvalidOperationException)
        {
        }
        catch (Exception)
        {
            Assert.Fail($"An {nameof(InvalidOperationException)} should have been thrown, but no exception was thrown.");
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserInfoWasNotReturnedFromUserInfoProvider_ThrowsInvalidOperationExceptionWhereMessageIsSpecificText()
    {
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut(hasUserInfo: false);

        UserInfoRequest request = CreateUserInfoRequest(_fixture!);
        try
        {
            await sut.ExecuteAsync(request);

            Assert.Fail($"An {nameof(InvalidOperationException)} should have been thrown, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Unable to retrieve user information from the user info provider."));
        }
        catch (Exception)
        {
            Assert.Fail($"An {nameof(InvalidOperationException)} should have been thrown, but no exception was thrown.");
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserInfoWasNotReturnedFromUserInfoProvider_ThrowsInvalidOperationExceptionWhereInnerExceptionIsNull()
    {
        IQueryFeature<UserInfoRequest, UserInfoResponse> sut = CreateSut(hasUserInfo: false);

        UserInfoRequest request = CreateUserInfoRequest(_fixture!);
        try
        {
            await sut.ExecuteAsync(request);

            Assert.Fail($"An {nameof(InvalidOperationException)} should have been thrown, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
        catch (Exception)
        {
            Assert.Fail($"An {nameof(InvalidOperationException)} should have been thrown, but no exception was thrown.");
        }
    }

    private IQueryFeature<UserInfoRequest, UserInfoResponse> CreateSut(bool hasUserInfo = true, IUserInfoModel? userInfoModel = null)
    {
        _userInfoProviderMock!.Setup(_fixture!, _random!, hasUserInfo: hasUserInfo, userInfoModel: userInfoModel);
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Security.UserInfo.UserInfoFeature(_permissionCheckerMock!.Object, _userInfoProviderMock!.Object, _staticTextProviderMock!.Object);
    }
}