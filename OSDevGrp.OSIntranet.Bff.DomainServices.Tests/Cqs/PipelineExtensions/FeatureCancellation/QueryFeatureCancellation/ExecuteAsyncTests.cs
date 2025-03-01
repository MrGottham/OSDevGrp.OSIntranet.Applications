using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureCancellation.QueryFeatureCancellation;

[TestFixture]
public class ExecuteAsyncTests : FeatureCancellationTestBase
{
    #region Private variables

    private Mock<IQueryFeature<IRequest, IResponse>>? _innerFeatureMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _innerFeatureMock = CreateQueryFeatureMock();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCancellationIsRequested_AssertExecuteAsyncWasNotCalledOnInnerFeature()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();

        IRequest request = CreateRequest(() => _fixture!);
        try
        {
            await sut.ExecuteAsync(request, cancellationToken);

            Assert.Fail("An OperationCanceledException was expected.");
        }
        catch (OperationCanceledException)
        {
            _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                    It.IsAny<IRequest>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCancellationIsRequested_ThrowsOperationCanceledException()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();

        IRequest request = CreateRequest(() => _fixture!);
        try
        {
            await sut.ExecuteAsync(request, cancellationToken);

            Assert.Fail("An OperationCanceledException was expected.");
        }
        catch (OperationCanceledException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCancellationIsNotRequested_AssertExecuteAsyncWasCalledOnInnerFeatureWithSameRequest()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IRequest>(value => value == request),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCancellationIsNotRequested_AssertExecuteAsyncWasCalledOnInnerFeatureWithCancellationToken()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<IRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCancellationIsNotRequested_ReturnsResponseFromInnerFeature()
    {
        IResponse response = CreateResponse();
        _innerFeatureMock = CreateQueryFeatureMock(response: response);
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        IRequest request = CreateRequest(() => _fixture!);
        IResponse result = await sut.ExecuteAsync(request, cancellationToken);

        Assert.That(result, Is.SameAs(response));
    }

     private IQueryFeature<IRequest, IResponse> CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureCancellation.QueryFeatureCancellation<IRequest, IResponse>(_innerFeatureMock!.Object);
    }
}