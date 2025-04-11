using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeaturePermissionVerifier.CommandFeaturePermissionVerifier;

[TestFixture]
public class ExecuteAsyncTests : FeaturePermissionVerifierTestBase
{
    #region Private variables

    private Mock<ICommandFeature<IRequest>>? _innerFeatureMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _innerFeatureMock = CreateCommandFeatureMock(isPermissionVerifiable: true);
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenInnerMostFeatureIsPermissionVerifiable_AssertSecurityContextWasCalledOnRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        Mock<IRequest> request = CreateRequestMock(() => _fixture!);
        await sut.ExecuteAsync(request.Object, CancellationToken.None);

        request.Verify(m => m.SecurityContext, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenInnerMostFeatureIsPermissionVerifiable_AssertVerifyPermissionAsyncWasCalledOnInnerMostFeatureWithSecurityContextFromRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        IRequest request = CreateRequest(() => _fixture!, securityContext: securityContext);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _innerFeatureMock!.As<IPermissionVerifiable>().Verify(m => m.VerifyPermissionAsync(
                It.Is<ISecurityContext>(value => value == securityContext),
                It.IsAny<IRequest>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenInnerMostFeatureIsPermissionVerifiable_AssertVerifyPermissionAsyncWasCalledOnInnerMostFeatureWithSameRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _innerFeatureMock!.As<IPermissionVerifiable>().Verify(m => m.VerifyPermissionAsync(
                It.IsAny<ISecurityContext>(),
                It.Is<IRequest>(value => value == request),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenInnerMostFeatureIsPermissionVerifiable_AssertVerifyPermissionAsyncWasCalledOnInnerMostFeatureWithSameCancellationToken()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock!.As<IPermissionVerifiable>().Verify(m => m.VerifyPermissionAsync(
                It.IsAny<ISecurityContext>(),
                It.IsAny<IRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhentVerifyPermissionAsyncOnInnerMostFeatureReturnsTrue_AssertExecuteAsyncWasCalledOnInnerFeatureWithSameRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IRequest>(value => value == request),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhentVerifyPermissionAsyncOnInnerMostFeatureReturnsTrue_AssertExecuteAsyncWasCalledOnInnerFeatureWithSameCancellationToken()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<IRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhentVerifyPermissionAsyncOnInnerMostFeatureReturnsFalse_AssertExecuteAsyncWasNotCalledOnInnerFeature()
    {
        _innerFeatureMock = CreateCommandFeatureMock(isPermissionVerifiable: true, permissionGranted: false);
        ICommandFeature<IRequest> sut = CreateSut();

        try
        {
            IRequest request = CreateRequest(() => _fixture!);
            await sut.ExecuteAsync(request, CancellationToken.None);

            Assert.Fail("An SecurityException should have been thrown.");
        }
        catch (SecurityException)
        {
            _innerFeatureMock.Verify(m => m.ExecuteAsync(
                    It.IsAny<IRequest>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhentVerifyPermissionAsyncOnInnerMostFeatureReturnsFalse_ThrowsSecurityExceptionWithExpectedMessage()
    {
        _innerFeatureMock = CreateCommandFeatureMock(isPermissionVerifiable: true, permissionGranted: false);
        ICommandFeature<IRequest> sut = CreateSut();

        try
        {
            IRequest request = CreateRequest(() => _fixture!);
            await sut.ExecuteAsync(request, CancellationToken.None);

            Assert.Fail("An SecurityException should have been thrown.");
        }
        catch (SecurityException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Access denied due to insufficient privileges."));
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhentVerifyPermissionAsyncOnInnerMostFeatureReturnsFalse_ThrowsSecurityExceptionWhereInnerExceptionIsNull()
    {
        _innerFeatureMock = CreateCommandFeatureMock(isPermissionVerifiable: true, permissionGranted: false);
        ICommandFeature<IRequest> sut = CreateSut();

        try
        {
            IRequest request = CreateRequest(() => _fixture!);
            await sut.ExecuteAsync(request, CancellationToken.None);

            Assert.Fail("An SecurityException should have been thrown.");
        }
        catch (SecurityException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenInnerMostFeatureIsNotPermissionVerifiable_AssertSecurityContextWasNotCalledOnRequest()
    {
        _innerFeatureMock = CreateCommandFeatureMock(isPermissionVerifiable: false);
        ICommandFeature<IRequest> sut = CreateSut();

        Mock<IRequest> request = CreateRequestMock(() => _fixture!);
        await sut.ExecuteAsync(request.Object, CancellationToken.None);

        request.Verify(m => m.SecurityContext, Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenInnerMostFeatureIsNotPermissionVerifiable_AssertExecuteAsyncWasCalledOnInnerFeatureWithSameRequest()
    {
        _innerFeatureMock = CreateCommandFeatureMock(isPermissionVerifiable: false);
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _innerFeatureMock.Verify(m => m.ExecuteAsync(
                It.Is<IRequest>(value => value == request),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenInnerMostFeatureIsNotPermissionVerifiable_AssertExecuteAsyncWasCalledOnInnerFeatureWithSameCancellationToken()
    {
        _innerFeatureMock = CreateCommandFeatureMock(isPermissionVerifiable: false);
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock.Verify(m => m.ExecuteAsync(
                It.IsAny<IRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    private ICommandFeature<IRequest> CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier.CommandFeaturePermissionVerifier<IRequest>(_innerFeatureMock!.Object);
    }
}