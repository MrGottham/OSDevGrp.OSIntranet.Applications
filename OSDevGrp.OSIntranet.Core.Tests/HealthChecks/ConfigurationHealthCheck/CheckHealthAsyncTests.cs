using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.ConfigurationHealthCheck
{
    [TestFixture]
    public class CheckHealthAsyncTests
    {
        #region Private variables

        private Mock<IOptions<ConfigurationHealthCheckTestOptions>> _optionsMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<ILogger> _loggerMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _optionsMock = new Mock<IOptions<ConfigurationHealthCheckTestOptions>>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerMock = new Mock<ILogger>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void CheckHealthAsync_WhenContextIsNull_ThrowsArgumentNullException()
        {
            IHealthCheck sut = CreateSut();

            // ReSharper disable AssignNullToNotNullAttribute
            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CheckHealthAsync(null));
            // ReSharper restore AssignNullToNotNullAttribute

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("context"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenCalled_AssertValueWasCalledOnOptions()
        {
            IHealthCheck sut = CreateSut();

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            _optionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenCalled_AssertValidateAsyncWasCalledOnEachConfigurationValueValidatorWithinConfigurationHealthCheckOptions()
        {
            IEnumerable<Mock<IConfigurationValueValidator>> configurationValueValidatorMockCollection = new[]
            {
                CreateConfigurationValueValidatorMock(),
                CreateConfigurationValueValidatorMock(),
                CreateConfigurationValueValidatorMock(),
                CreateConfigurationValueValidatorMock(),
                CreateConfigurationValueValidatorMock()
            };
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(configurationValueValidatorMockCollection.Select(m => m.Object).ToArray());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            foreach (Mock<IConfigurationValueValidator> configurationValueValidatorMock in configurationValueValidatorMockCollection)
            {
                configurationValueValidatorMock.Verify(m => m.ValidateAsync(), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_AssertCreateLoggerWasNotCalledOnLoggerFactory()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_AssertLogWasNotCalledOnLoggerFromLoggerFactory()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            _loggerMock.Verify(m => m.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_ReturnsNotNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_ReturnsHealthCheckResult()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result, Is.TypeOf<HealthCheckResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_ReturnsHealthCheckResultWhereStatusIsEqualToHealthy()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_ReturnsHealthCheckResultWhereDescriptionIsNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Description, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_ReturnsHealthCheckResultWhereExceptionIsNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Exception, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_ReturnsHealthCheckResultWhereDataIsNotNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenConfigurationHealthCheckOptionsDoesNotContainAnyConfigurationValueValidators_ReturnsHealthCheckResultWhereDataIsNotEmpty()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(Array.Empty<IConfigurationValueValidator>());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_AssertCreateLoggerWasNotCalledOnLoggerFactory()
        {
            IHealthCheck sut = CreateSut();

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_AssertLogWasNotCalledOnLoggerFromLoggerFactory()
        {
            IHealthCheck sut = CreateSut();

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            _loggerMock.Verify(m => m.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsNotNull()
        {
            IHealthCheck sut = CreateSut();

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResult()
        {
            IHealthCheck sut = CreateSut();

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result, Is.TypeOf<HealthCheckResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereStatusIsEqualToHealthy()
        {
            IHealthCheck sut = CreateSut();

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDescriptionIsNull()
        {
            IHealthCheck sut = CreateSut();

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Description, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereExceptionIsNull()
        {
            IHealthCheck sut = CreateSut();

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Exception, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDataIsNotNull()
        {
            IHealthCheck sut = CreateSut();

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenNoExceptionWasThrownByAnyConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDataIsNotEmpty()
        {
            IHealthCheck sut = CreateSut();

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_AssertCreateLoggerWasCalledOnLoggerFactoryForEachException()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            Type configurationHealthCheckType = typeof(ConfigurationHealthCheck<>);
            string expectedCategoryName = $"{configurationHealthCheckType.Namespace}.{configurationHealthCheckType.Name.Substring(0, configurationHealthCheckType.Name.IndexOf("`", StringComparison.InvariantCulture))}";

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.Is<string>(value => string.CompareOrdinal(value, expectedCategoryName) == 0)), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_AssertLogWasCalledOnLoggerFromLoggerFactoryWithEachException()
        {
            Exception firstException = _fixture.Create<Exception>();
            Exception secondException = _fixture.Create<Exception>();
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(firstException),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(secondException),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            await sut.CheckHealthAsync(CreateHealthCheckContext());

            _loggerMock.Verify(m => m.Log(It.Is<LogLevel>(value => value == LogLevel.Error), It.IsNotNull<EventId>(), It.IsNotNull<It.IsAnyType>(), It.Is<Exception>(value => value != null && value == firstException), It.IsNotNull<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            _loggerMock.Verify(m => m.Log(It.Is<LogLevel>(value => value == LogLevel.Error), It.IsNotNull<EventId>(), It.IsNotNull<It.IsAnyType>(), It.Is<Exception>(value => value != null && value == secondException), It.IsNotNull<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsNotNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResult()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result, Is.TypeOf<HealthCheckResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereStatusIsEqualToUnhealthy()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Description, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Description, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDescriptionIsContainsMessageFromEachException()
        {
            Exception firstException = _fixture.Create<Exception>();
            Exception secondException = _fixture.Create<Exception>();
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(firstException),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(secondException),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Description.Contains(firstException.Message), Is.True);
            Assert.That(result.Description.Contains(secondException.Message), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereExceptionIsNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Exception, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDataIsNotNull()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CheckHealthAsync_WhenExceptionWasThrownByOneOrMoreConfigurationValueValidatorWithinConfigurationHealthCheckOptions_ReturnsHealthCheckResultWhereDataIsNotEmpty()
        {
            ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = CreateConfigurationHealthCheckTestOptions(
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator(),
                CreateConfigurationValueValidator(_fixture.Create<Exception>()),
                CreateConfigurationValueValidator());
            IHealthCheck sut = CreateSut(configurationHealthCheckTestOptions);

            HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

            Assert.That(result.Data, Is.Empty);
        }

        private IHealthCheck CreateSut(ConfigurationHealthCheckTestOptions configurationHealthCheckTestOptions = null)
        {
            _optionsMock.Setup(m => m.Value)
                .Returns(configurationHealthCheckTestOptions ?? CreateConfigurationHealthCheckTestOptions());

            _loggerFactoryMock.Setup(m => m.CreateLogger(It.IsAny<string>()))
                .Returns(_loggerMock.Object);

            return new ConfigurationHealthCheck<ConfigurationHealthCheckTestOptions>(_optionsMock.Object, _loggerFactoryMock.Object);
        }

        private ConfigurationHealthCheckTestOptions CreateConfigurationHealthCheckTestOptions()
        {
            return CreateConfigurationHealthCheckTestOptions(CreateConfigurationValueValidator(), CreateConfigurationValueValidator(), CreateConfigurationValueValidator(), CreateConfigurationValueValidator(), CreateConfigurationValueValidator());
        }

        private ConfigurationHealthCheckTestOptions CreateConfigurationHealthCheckTestOptions(params IConfigurationValueValidator[] configurationValueValidatorCollection)
        {
            Core.NullGuard.NotNull(configurationValueValidatorCollection,
                nameof(configurationValueValidatorCollection));

            return new ConfigurationHealthCheckTestOptions(configurationValueValidatorCollection);
        }

        private IConfigurationValueValidator CreateConfigurationValueValidator(Exception exception = null)
        {
            return CreateConfigurationValueValidatorMock(exception).Object;
        }

        private Mock<IConfigurationValueValidator> CreateConfigurationValueValidatorMock(Exception exception = null)
        {
            Mock<IConfigurationValueValidator> configurationValueValidatorMock = new Mock<IConfigurationValueValidator>();
            if (exception != null)
            {
                configurationValueValidatorMock.Setup(m => m.ValidateAsync())
                    .Throws(exception);
            }
            else
            {
                configurationValueValidatorMock.Setup(m => m.ValidateAsync())
                    .Returns(Task.CompletedTask);
            }
            return configurationValueValidatorMock;
        }

        private HealthCheckContext CreateHealthCheckContext()
        {
            return new HealthCheckContext();
        }
    }
}