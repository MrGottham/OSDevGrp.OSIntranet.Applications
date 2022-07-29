using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.ConnectionStringValidator
{
    [TestFixture]
    public class ValidateAsyncTests
    {
        #region Private variables

        private Mock<IConfigurationSection> _connectionStringsMock;
        private Mock<IConfiguration> _configurationMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _connectionStringsMock = new Mock<IConfigurationSection>();
            _configurationMock = new Mock<IConfiguration>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenCalled_AssertGetSectionWasCalledOnConfigurationWithConnectionStrings()
        {
            IConfigurationValueValidator sut = CreateSut();

            await sut.ValidateAsync();

            _configurationMock.Verify(m => m.GetSection(It.Is<string>(value => string.Compare(value, "connectionStrings", true) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotHaveConfigurationSectionForConnectionStrings_ThrowsIntranetSystemException()
        {
            IConfigurationValueValidator sut = CreateSut(hasConfigurationSectionForConnectionStrings: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotHaveConfigurationSectionForConnectionStrings_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            IConfigurationValueValidator sut = CreateSut(hasConfigurationSectionForConnectionStrings: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotHaveConfigurationSectionForConnectionStrings_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty()
        {
            IConfigurationValueValidator sut = CreateSut(hasConfigurationSectionForConnectionStrings: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotHaveConfigurationSectionForConnectionStrings_ThrowsIntranetSystemExceptionWhereMessageContainsName()
        {
            string name = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(name, false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message.Contains($"'{name}'"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotHaveConfigurationSectionForConnectionStrings_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToMissingConnectionString()
        {
            IConfigurationValueValidator sut = CreateSut(hasConfigurationSectionForConnectionStrings: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.MissingConnectionString));
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationDoesNotHaveConfigurationSectionForConnectionStrings_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            IConfigurationValueValidator sut = CreateSut(hasConfigurationSectionForConnectionStrings: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenConfigurationHasConfigurationSectionForConnectionStrings_AssertItemWasCalledOnConfigurationSectionForConnectionStringsWithName()
        {
            string name = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(name);

            await sut.ValidateAsync();

            _connectionStringsMock.Verify(m => m[It.Is<string>(value => string.CompareOrdinal(value, name) == 0)], Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationSectionForConnectionStringsDoesNotContainName_ThrowsIntranetSystemException()
        {
            IConfigurationValueValidator sut = CreateSut(hasConnectionString: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationSectionForConnectionStringsDoesNotContainName_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            IConfigurationValueValidator sut = CreateSut(hasConnectionString: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationSectionForConnectionStringsDoesNotContainName_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty()
        {
            IConfigurationValueValidator sut = CreateSut(hasConnectionString: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationSectionForConnectionStringsDoesNotContainName_ThrowsIntranetSystemExceptionWhereMessageContainsName()
        {
            string name = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(name, hasConnectionString: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message.Contains($"'{name}'"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationSectionForConnectionStringsDoesNotContainName_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToMissingConnectionString()
        {
            IConfigurationValueValidator sut = CreateSut(hasConnectionString: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.MissingConnectionString));
        }

        [Test]
        [Category("UnitTest")]
        public void ValidateAsync_WhenConfigurationSectionForConnectionStringsDoesNotContainName_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            IConfigurationValueValidator sut = CreateSut(hasConnectionString: false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenValueForNamedConnectionStringIsEmptyOrWhiteSpace_ThrowsIntranetSystemException(string value)
        {
            IConfigurationValueValidator sut = CreateSut(connectionString: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenValueForNamedConnectionStringIsEmptyOrWhiteSpace_ThrowsIntranetSystemExceptionWhereMessageIsNotNull(string value)
        {
            IConfigurationValueValidator sut = CreateSut(connectionString: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenValueForNamedConnectionStringIsEmptyOrWhiteSpace_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty(string value)
        {
            IConfigurationValueValidator sut = CreateSut(connectionString: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenValueForNamedConnectionStringIsEmptyOrWhiteSpace_ThrowsIntranetSystemExceptionWhereMessageContainsName(string value)
        {
            string name = _fixture.Create<string>();
            IConfigurationValueValidator sut = CreateSut(name, connectionString: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.Message.Contains($"'{name}'"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenValueForNamedConnectionStringIsEmptyOrWhiteSpace_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToMissingConnectionString(string value)
        {
            IConfigurationValueValidator sut = CreateSut(connectionString: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.MissingConnectionString));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateAsync_WhenValueForNamedConnectionStringIsEmptyOrWhiteSpace_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull(string value)
        {
            IConfigurationValueValidator sut = CreateSut(connectionString: value);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ValidateAsync());

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ValidateAsync_WhenValueForNamedConnectionStringIsNotEmptyOrWhiteSpace_ExpectNoError()
        {
            IConfigurationValueValidator sut = CreateSut();

            await sut.ValidateAsync();
        }

        private IConfigurationValueValidator CreateSut(string name = null, bool hasConfigurationSectionForConnectionStrings = true, bool hasConnectionString = true, string connectionString = null)
        {
            _connectionStringsMock.Setup(m => m[It.IsAny<string>()])
                .Returns(hasConnectionString ? connectionString ?? _fixture.Create<string>() : null);

            _configurationMock.Setup(m => m.GetSection(It.Is<string>(value => string.Compare(value, "connectionStrings", true) == 0)))
                .Returns(hasConfigurationSectionForConnectionStrings ? _connectionStringsMock.Object : null);

            return new OSDevGrp.OSIntranet.Core.HealthChecks.ConnectionStringValidator(_configurationMock.Object, name ?? _fixture.Create<string>());
        }
    }
}