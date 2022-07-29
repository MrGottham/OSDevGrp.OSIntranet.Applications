using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.StringCollectionConfigurationValidator
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
        [TestCase("XXX", 2)]
        [TestCase("XXX;YYY", 3)]
        [TestCase("XXX;YYY;ZZZ", 4)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsLowerThanMinLength_ThrowsIntranetSystemException(string value, int minLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, minLength: minLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 2)]
        [TestCase("XXX;YYY", 3)]
        [TestCase("XXX;YYY;ZZZ", 4)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsLowerThanMinLength_ThrowsIntranetSystemExceptionWhereMessageIsNotNull(string value, int minLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, minLength: minLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 2)]
        [TestCase("XXX;YYY", 3)]
        [TestCase("XXX;YYY;ZZZ", 4)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsLowerThanMinLength_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty(string value, int minLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, minLength: minLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 2)]
        [TestCase("XXX;YYY", 3)]
        [TestCase("XXX;YYY;ZZZ", 4)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsLowerThanMinLength_ThrowsIntranetSystemExceptionWhereMessageContainsKey(string value, int minLength)
        {
            string key = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(key, value: value, minLength: minLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message.Contains($"'{key}'"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 2)]
        [TestCase("XXX;YYY", 3)]
        [TestCase("XXX;YYY;ZZZ", 4)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsLowerThanMinLength_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToInvalidConfigurationValue(string value, int minLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, minLength: minLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.InvalidConfigurationValue));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 2)]
        [TestCase("XXX;YYY", 3)]
        [TestCase("XXX;YYY;ZZZ", 4)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsLowerThanMinLength_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull(string value, int minLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, minLength: minLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 0)]
        [TestCase("XXX;YYY", 1)]
        [TestCase("XXX;YYY;ZZZ", 2)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsMoreThanMaxLength_ThrowsIntranetSystemException(string value, int maxLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, maxLength: maxLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 0)]
        [TestCase("XXX;YYY", 1)]
        [TestCase("XXX;YYY;ZZZ", 2)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsMoreThanMaxLength_ThrowsIntranetSystemExceptionWhereMessageIsNotNull(string value, int maxLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, maxLength: maxLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 0)]
        [TestCase("XXX;YYY", 1)]
        [TestCase("XXX;YYY;ZZZ", 2)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsMoreThanMaxLength_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty(string value, int maxLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, maxLength: maxLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 0)]
        [TestCase("XXX;YYY", 1)]
        [TestCase("XXX;YYY;ZZZ", 2)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsMoreThanMaxLength_ThrowsIntranetSystemExceptionWhereMessageContainsKey(string value, int maxLength)
        {
            string key = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(key, value: value, maxLength: maxLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message.Contains($"'{key}'"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 0)]
        [TestCase("XXX;YYY", 1)]
        [TestCase("XXX;YYY;ZZZ", 2)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsMoreThanMaxLength_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToInvalidConfigurationValue(string value, int maxLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, maxLength: maxLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.InvalidConfigurationValue));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX", 0)]
        [TestCase("XXX;YYY", 1)]
        [TestCase("XXX;YYY;ZZZ", 2)]
        public void ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsMoreThanMaxLength_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull(string value, int maxLength)
        {
            IConfigurationValueValidator sut = CreateSut(value: value, maxLength: maxLength);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenConfigurationContainsKeyWithValueWhichHasElementsBetweenMinLengthAndMaxLength_ExpectNoError()
        {
            string value = $"{_fixture.Create<string>().Replace(";", string.Empty)};{_fixture.Create<string>().Replace(";", string.Empty)};{_fixture.Create<string>().Replace(";", string.Empty)}";
            IConfigurationValueValidator sut = CreateSut(value: value, minLength: 1, maxLength: 3);

            await sut.ValidateAsync();
        }

        private IConfigurationValueValidator CreateSut(string key = null, bool hasKey = true, string value = null, int minLength = 1, int maxLength = 32)
        {
            _configurationMock.Setup(m => m[It.IsAny<string>()])
                .Returns(hasKey ? value ?? $"{_fixture.Create<string>().Replace(";", string.Empty)};{_fixture.Create<string>().Replace(";", string.Empty)};{_fixture.Create<string>().Replace(";", string.Empty)}" : null);

            return new OSDevGrp.OSIntranet.Core.HealthChecks.StringCollectionConfigurationValidator(_configurationMock.Object, key ?? _fixture.Create<string>(), ";", minLength, maxLength);
        }
    }
}