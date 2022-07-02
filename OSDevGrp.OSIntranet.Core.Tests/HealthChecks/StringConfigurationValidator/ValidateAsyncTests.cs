using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.StringConfigurationValidator
{
    [TestFixture]
    public class ValidateAsyncTests
    {
        #region Private variables

        private Mock<IConfiguration> _configurationMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _configurationMock = new Mock<IConfiguration>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenCalled_AssertItemWasCalledOnConfigurationWithKey()
        {
            string key = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(key);

            await sut.ValidateAsync();

            _configurationMock.Verify(m => m[It.Is<string>(value => string.CompareOrdinal(value, key) == 0)], Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotContainKey_AssertValidateAsyncWithValueWasNotCalledOnSut()
        {
            IConfigurationValueValidator sut = CreateSut(hasKey: false);

            Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(((Sut) sut).ValidateAsyncWithValueWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotContainKey_ThrowsIntranetSystemException()
        {
            IConfigurationValueValidator sut = CreateSut(hasKey: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotContainKey_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            IConfigurationValueValidator sut = CreateSut(hasKey: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotContainKey_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty()
        {
            IConfigurationValueValidator sut = CreateSut(hasKey: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotContainKey_ThrowsIntranetSystemExceptionWhereMessageContainsKey()
        {
            string key = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(key, false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message.Contains($"'{key}'"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotContainKey_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToMissingConfiguration()
        {
            IConfigurationValueValidator sut = CreateSut(hasKey: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.MissingConfiguration));
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotContainKey_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            IConfigurationValueValidator sut = CreateSut(hasKey: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenConfigurationContainsKeyWithoutValue_AssertValidateAsyncWithValueWasNotCalledOnSut(string value)
        {
            IConfigurationValueValidator sut = CreateSut(value: value);

            Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(((Sut) sut).ValidateAsyncWithValueWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenConfigurationContainsKeyWithoutValue_ThrowsIntranetSystemException(string value)
        {
            IConfigurationValueValidator sut = CreateSut(value: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenConfigurationContainsKeyWithoutValue_ThrowsIntranetSystemExceptionWhereMessageIsNotNull(string value)
        {
            IConfigurationValueValidator sut = CreateSut(value: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenConfigurationContainsKeyWithoutValue_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty(string value)
        {
            IConfigurationValueValidator sut = CreateSut(value: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenConfigurationContainsKeyWithoutValue_ThrowsIntranetSystemExceptionWhereMessageContainsKey(string value)
        {
            string key = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(key, value: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message.Contains($"'{key}'"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenConfigurationContainsKeyWithoutValue_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToMissingConfiguration(string value)
        {
            IConfigurationValueValidator sut = CreateSut(value: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.MissingConfiguration));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenConfigurationContainsKeyWithoutValue_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull(string value)
        {
            IConfigurationValueValidator sut = CreateSut(value: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenConfigurationContainsKeyWithValue_AssertValidateAsyncWithValueWasCalledOnSut()
        {
            IConfigurationValueValidator sut = CreateSut();

            await sut.ValidateAsync();

            Assert.That(((Sut) sut).ValidateAsyncWithValueWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenConfigurationContainsKeyWithValue_AssertValidateAsyncWithValueWasCalledOnSutWithValue()
        {
            string value = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(value: value);

            await sut.ValidateAsync();

            Assert.That(((Sut) sut).ValidateAsyncWithValueCalledWith, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenConfigurationContainsKeyWithValue_ExpectNoError()
        {
            IConfigurationValueValidator sut = CreateSut();

            await sut.ValidateAsync();
        }

        private IConfigurationValueValidator CreateSut(string key = null, bool hasKey = true, string value = null)
        {
            _configurationMock.Setup(m => m[It.IsAny<string>()])
                .Returns(hasKey ? value ?? _fixture.Create<string>() : null);

            return new Sut(_configurationMock.Object, key ?? _fixture.Create<string>());
        }

        private class Sut : OSDevGrp.OSIntranet.Core.HealthChecks.StringConfigurationValidator
        {
            #region Constructor

            public Sut(IConfiguration configuration, string key)
                : base(configuration, key)
            {
            }

            #endregion

            #region Properties

            public bool ValidateAsyncWithValueWasCalled { get; private set; }

            public string ValidateAsyncWithValueCalledWith { get; private set; }

            #endregion

            #region Methods

            protected override Task ValidateAsync(string value)
            {
                OSDevGrp.OSIntranet.Core.NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                ValidateAsyncWithValueWasCalled = true;
                ValidateAsyncWithValueCalledWith = value;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}