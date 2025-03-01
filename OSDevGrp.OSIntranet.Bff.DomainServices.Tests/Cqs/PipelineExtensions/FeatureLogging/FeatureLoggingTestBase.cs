using Microsoft.Extensions.Logging;
using Moq;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureLogging;

public abstract class FeatureLoggingTestBase : PipelineExtensionTestBase
{
    #region Methods

    protected static ILoggerFactory CreateLoggerFactory(Func<ILogger> loggerGetter)
    {
        return CreateLoggerFactoryMock(loggerGetter).Object;
    }

    protected static Mock<ILoggerFactory> CreateLoggerFactoryMock(Func<ILogger> loggerGetter)
    {
        Mock<ILoggerFactory> loggerFactoryMock = new Mock<ILoggerFactory>();
        loggerFactoryMock.Setup(m => m.CreateLogger(It.IsAny<string>()))
            .Returns(loggerGetter());
        return loggerFactoryMock;
    }

    protected static ILogger CreateLogger()
    {
        return CreateLoggerMock().Object;
    }

    protected static Mock<ILogger> CreateLoggerMock()
    {
        return new Mock<ILogger>();
    }

    #endregion
}