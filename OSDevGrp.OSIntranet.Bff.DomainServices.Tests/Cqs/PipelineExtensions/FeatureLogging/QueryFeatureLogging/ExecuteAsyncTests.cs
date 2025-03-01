using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureLogging.QueryFeatureLogging;

[TestFixture]
public class ExecuteAsyncTests : FeatureLoggingTestBase
{
    #region Private variables

    private Mock<IQueryFeature<IRequest, IResponse>>? _innerFeatureMock;
    private Mock<ILoggerFactory>? _loggerFactoryMock;
    private Mock<ILogger>? _loggerMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _innerFeatureMock = CreateQueryFeatureMock();
        _loggerMock = CreateLoggerMock();
        _loggerFactoryMock = CreateLoggerFactoryMock(() => 
        {
            _loggerMock = CreateLoggerMock();
            return _loggerMock.Object;
        });
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertCreateLoggerWasCalledOnLoggerFactoryWithTypeNameForInnerFeature()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _loggerFactoryMock!.Verify(m => m.CreateLogger(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && value == $"{_innerFeatureMock!.Object.GetType().Namespace}.{_innerFeatureMock!.Object.GetType().Name}")), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertRequestIdWasCalledOnRequest()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        Mock<IRequest> requestMock = CreateRequestMock(() => _fixture!);
        await sut.ExecuteAsync(requestMock.Object, CancellationToken.None);

        requestMock.Verify(m => m.RequestId, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertLogWasCalledOnLoggerWithStartingExecution()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        Guid requestId = Guid.NewGuid();
        IRequest request = CreateRequest(() => _fixture!, requestId: requestId);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _loggerMock!.Verify(m => m.Log(
                It.Is<LogLevel>(value => value == LogLevel.Debug),
                It.Is<EventId>(value => value.Id == 0),
                It.Is<It.IsAnyType>((obj, _) => obj.ToString() == $"Starting executing of query feature {_innerFeatureMock!.Object.GetType().Namespace}.{_innerFeatureMock!.Object.GetType().Name} for request ID {requestId}"),
                It.Is<Exception?>(value => value == null),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledOnInnerFeatureWithSameRequest()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IRequest>(value => value == request),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledOnInnerFeatureWithSameCancellationToken()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

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
    public async Task ExecuteAsync_WhenNoExceptionWasThrown_AssertLogWasNotCalledOnLoggerWithError()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _loggerMock!.Verify(m => m.Log(
                It.Is<LogLevel>(value => value == LogLevel.Error),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenNoExceptionWasThrown_AssertLogWasCalledOnLoggerWithFinishingExecution()
    {
        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        Guid requestId = Guid.NewGuid();
        IRequest request = CreateRequest(() => _fixture!, requestId: requestId);
        await sut.ExecuteAsync(request, CancellationToken.None);

        _loggerMock!.Verify(m => m.Log(
                It.Is<LogLevel>(value => value == LogLevel.Debug),
                It.Is<EventId>(value => value.Id == 0),
                It.Is<It.IsAnyType>((obj, _) => obj.ToString() == $"Finishing executing of query feature {_innerFeatureMock!.Object.GetType().Namespace}.{_innerFeatureMock!.Object.GetType().Name} for request ID {requestId}"),
                It.Is<Exception?>(value => value == null),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenNoExceptionWasThrown_ReturnsResponseFromInnerFeature()
    {
        IResponse response = CreateResponse();
        _innerFeatureMock = CreateQueryFeatureMock(response: response);

        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        IRequest request = CreateRequest(() => _fixture!);
        IResponse result = await sut.ExecuteAsync(request, CancellationToken.None);

        Assert.That(result, Is.SameAs(response));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenExceptionWasThrown_AssertLogWasCalledOnLoggerWithError()
    {
        InvalidOperationException exception = new InvalidOperationException(_fixture.Create<string>());
        _innerFeatureMock = CreateFailingQueryFeatureMock(() => exception);

        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        Guid requestId = Guid.NewGuid();
        try
        {
            IRequest request = CreateRequest(() => _fixture!, requestId: requestId);
            await sut.ExecuteAsync(request, CancellationToken.None);

            Assert.Fail("An InvalidOperationException should have been thrown.");
        }
        catch (InvalidOperationException)
        {
            _loggerMock!.Verify(m => m.Log(
                    It.Is<LogLevel>(value => value == LogLevel.Error),
                    It.Is<EventId>(value => value.Id == 0),
                    It.Is<It.IsAnyType>((obj, _) => obj.ToString() == $"Error while executing of query feature {_innerFeatureMock!.Object.GetType().Namespace}.{_innerFeatureMock!.Object.GetType().Name} for request ID {requestId}: {exception}"),
                    It.Is<Exception?>(value => value == exception),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenExceptionWasThrown_AssertLogWasCalledOnLoggerWithFinishingExecution()
    {
        InvalidOperationException exception = new InvalidOperationException(_fixture.Create<string>());
        _innerFeatureMock = CreateFailingQueryFeatureMock(() => exception);

        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        Guid requestId = Guid.NewGuid();
        try
        {
            IRequest request = CreateRequest(() => _fixture!, requestId: requestId);
            await sut.ExecuteAsync(request, CancellationToken.None);

            Assert.Fail("An InvalidOperationException should have been thrown.");
        }
        catch (InvalidOperationException)
        {
            _loggerMock!.Verify(m => m.Log(
                    It.Is<LogLevel>(value => value == LogLevel.Debug),
                    It.Is<EventId>(value => value.Id == 0),
                    It.Is<It.IsAnyType>((obj, _) => obj.ToString() == $"Finishing executing of query feature {_innerFeatureMock!.Object.GetType().Namespace}.{_innerFeatureMock!.Object.GetType().Name} for request ID {requestId}"),
                    It.Is<Exception?>(value => value == null),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenExceptionWasThrown_ThrowsSameException()
    {
        InvalidOperationException exception = new InvalidOperationException(_fixture.Create<string>());
        _innerFeatureMock = CreateFailingQueryFeatureMock(() => exception);

        IQueryFeature<IRequest, IResponse> sut = CreateSut();

        Guid requestId = Guid.NewGuid();
        try
        {
            IRequest request = CreateRequest(() => _fixture!, requestId: requestId);
            await sut.ExecuteAsync(request, CancellationToken.None);

            Assert.Fail("An InvalidOperationException should have been thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex, Is.SameAs(exception));
        }
    }

    private IQueryFeature<IRequest, IResponse> CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureLogging.QueryFeatureLogging<IRequest, IResponse>(_innerFeatureMock!.Object, _loggerFactoryMock!.Object);
    }
}