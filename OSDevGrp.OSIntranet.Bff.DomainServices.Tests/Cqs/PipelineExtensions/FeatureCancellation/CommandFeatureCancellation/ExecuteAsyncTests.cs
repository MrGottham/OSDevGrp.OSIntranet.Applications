using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureCancellation.CommandFeatureCancellation;

[TestFixture]
public class ExecuteAsyncTests : FeatureCancellationTestBase
{
    #region Private variables

    private Mock<ICommandFeature<IRequest>>? _innerFeatureMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _innerFeatureMock = CreateCommandFeatureMock();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCancellationIsRequested_AssertExecuteAsyncWasNotCalledOnInnerFeature()
    {
        ICommandFeature<IRequest> sut = CreateSut();

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
        ICommandFeature<IRequest> sut = CreateSut();

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
        ICommandFeature<IRequest> sut = CreateSut();

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
        ICommandFeature<IRequest> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<IRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    private ICommandFeature<IRequest> CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureCancellation.CommandFeatureCancellation<IRequest>(_innerFeatureMock!.Object);
    }
}