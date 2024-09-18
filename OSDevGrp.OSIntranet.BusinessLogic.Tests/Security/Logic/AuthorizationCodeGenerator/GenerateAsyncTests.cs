using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthorizationCodeGenerator
{
    [TestFixture]
    public class GenerateAsyncTests
    {
        #region Private variables

        private Mock<IKeyGenerator> _keyGeneratorMock;
        private Mock<IAuthorizationCodeFactory> _authorizationCodeFactoryMock;
        private Mock<IAuthorizationCodeBuilder> _authorizationCodeBuilderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _keyGeneratorMock = new Mock<IKeyGenerator>();
            _authorizationCodeFactoryMock = new Mock<IAuthorizationCodeFactory>();
            _authorizationCodeBuilderMock = new Mock<IAuthorizationCodeBuilder>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertGenerateGenericKeyAsyncWasCalledOnKeyGeneratorWithStringCollection()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _keyGeneratorMock.Verify(m => m.GenerateGenericKeyAsync(It.IsNotNull<IEnumerable<string>>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertGenerateGenericKeyAsyncWasCalledOnKeyGeneratorWithNonEmptyStringCollection()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _keyGeneratorMock.Verify(m => m.GenerateGenericKeyAsync(It.Is<IEnumerable<string>>(value => value != null && value.Any())), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertGenerateGenericKeyAsyncWasCalledOnKeyGeneratorWithStringCollectionContainingTwoStrings()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _keyGeneratorMock.Verify(m => m.GenerateGenericKeyAsync(It.Is<IEnumerable<string>>(value => value != null && value.Count() == 2)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertGenerateGenericKeyAsyncWasCalledOnKeyGeneratorWithStringCollectionWhereFirstStringRepresentsGuid()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _keyGeneratorMock.Verify(m => m.GenerateGenericKeyAsync(It.Is<IEnumerable<string>>(value => value != null && IsGuid(value.ElementAt(0)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertGenerateGenericKeyAsyncWasCalledOnKeyGeneratorWithStringCollectionWhereSecondStringRepresentsIso8601DateTime()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _keyGeneratorMock.Verify(m => m.GenerateGenericKeyAsync(It.Is<IEnumerable<string>>(value => value != null && IsIso8601DateTime(value.ElementAt(1)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertGenerateGenericKeyAsyncWasCalledOnKeyGeneratorWithStringCollectionWhereSecondStringRepresentsDateTimeWithinNext10Minutes()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _keyGeneratorMock.Verify(m => m.GenerateGenericKeyAsync(It.Is<IEnumerable<string>>(value => value != null && IsDateTimeWithinMinutes(value.ElementAt(1), 10))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertCreateWasCalledOnAuthorizationCodeFactoryWithGenericKeyFromKeyGenerator()
        {
            string genericKey = _fixture.Create<string>();
            IAuthorizationCodeGenerator sut = CreateSut(genericKey: genericKey);

            await sut.GenerateAsync();

            _authorizationCodeFactoryMock.Verify(m => m.Create(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, genericKey) == 0),
                    It.IsAny<DateTimeOffset>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertCreateWasCalledOnAuthorizationCodeFactoryWithDateTimeOffsetWithin1Next0Minutes()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _authorizationCodeFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.Is<DateTimeOffset>(value => IsDateTimeWithinMinutes(value, 10))),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_AssertBuildWasCalledOnAuthorizationCodeBuilderCreatedByAuthorizationCodeFactory()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            await sut.GenerateAsync();

            _authorizationCodeBuilderMock.Verify(m => m.Build(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationCodeGenerator sut = CreateSut();

            IAuthorizationCode result = await sut.GenerateAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GenerateAsync_WhenCalled_ReturnsAuthorizationCodeFromAuthorizationCodeBuilderCreatedByAuthorizationCodeFactory()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IAuthorizationCodeGenerator sut = CreateSut(authorizationCode: authorizationCode);

            IAuthorizationCode result = await sut.GenerateAsync();

            Assert.That(result, Is.Not.Null);
        }

        private IAuthorizationCodeGenerator CreateSut(string genericKey = null, IAuthorizationCode authorizationCode = null)
        {
            _keyGeneratorMock.Setup(m => m.GenerateGenericKeyAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(genericKey ?? _fixture.Create<string>()));

            _authorizationCodeFactoryMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                .Returns(_authorizationCodeBuilderMock.Object);

            _authorizationCodeBuilderMock.Setup(m => m.Build())
                .Returns(authorizationCode ?? _fixture.BuildAuthorizationCodeMock().Object);

            return new BusinessLogic.Security.Logic.AuthorizationCodeGenerator(_keyGeneratorMock.Object, _authorizationCodeFactoryMock.Object);
        }

        private static bool IsGuid(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return Regex.IsMatch(value, RegexTestHelper.GuidPattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(16));
        }

        private static bool IsIso8601DateTime(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return Regex.IsMatch(value, RegexTestHelper.Iso8601DateTimePattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(16));
        }

        private static bool IsDateTimeWithinMinutes(string value, int minutes)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return IsDateTimeWithinMinutes(DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal), minutes);
        }

        private static bool IsDateTimeWithinMinutes(DateTimeOffset value, int minutes)
        {
            return IsDateTimeWithinMinutes(value.UtcDateTime, minutes);
        }

        private static bool IsDateTimeWithinMinutes(DateTime value, int minutes)
        {
            return value >= DateTime.UtcNow.AddSeconds(minutes * 60 - 10) && value <= DateTime.UtcNow.AddSeconds(minutes * 60);
        }
    }
}