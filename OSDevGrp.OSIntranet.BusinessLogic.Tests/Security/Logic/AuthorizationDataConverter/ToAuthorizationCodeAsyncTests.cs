using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthorizationDataConverter
{
    [TestFixture]
    public class ToAuthorizationCodeAsyncTests : AuthorizationDataConverterTestBase
    {
        #region Private variables

        private Mock<IAuthorizationCodeFactory> _authorizationCodeFactoryMock;
        private Mock<IAuthorizationCodeBuilder> _authorizationCodeBuilderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        [SetUp]
        public void SetUp()
        {
            _authorizationCodeFactoryMock = new Mock<IAuthorizationCodeFactory>();
            _authorizationCodeBuilderMock = new Mock<IAuthorizationCodeBuilder>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToAuthorizationCodeAsync_WhenKeyValueEntryIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationDataConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToAuthorizationCodeAsync(null, out IEnumerable<Claim> _));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("keyValueEntry"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_AssertCreateWasCalledOnAuthorizationCodeFactoryWithValueForAuthorizationCodeInKeyValueEntry()
        {
            IAuthorizationDataConverter sut = CreateSut();

            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut, authorizationCode: authorizationCode);
            await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> _);

            _authorizationCodeFactoryMock.Verify(m => m.Create(
                    It.Is<string>(val => string.IsNullOrWhiteSpace(val) == false && string.CompareOrdinal(val, value) == 0), 
                    It.IsAny<DateTimeOffset>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_AssertCreateWasCalledOnAuthorizationCodeFactoryWithExpiresForAuthorizationCodeInKeyValueEntry()
        {
            IAuthorizationDataConverter sut = CreateSut();

            DateTimeOffset expires = DateTimeOffset.UtcNow.AddSeconds(_random.Next(60, 600));
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expires: expires).Object;
            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut, authorizationCode: authorizationCode);
            await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> _);

            _authorizationCodeFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.Is<DateTimeOffset>(value => value == expires)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_AssertBuildWasCalledOnAuthorizationCodeBuilderCreatedByAuthorizationCodeFactory()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut);
            await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> _);

            _authorizationCodeBuilderMock.Verify(m => m.Build(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut);
            IAuthorizationCode result = await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> _);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_ReturnsAuthorizationCodeCreateByAuthorizationCodeBuilder()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IAuthorizationDataConverter sut = CreateSut(authorizationCode: authorizationCode);

            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut);
            IAuthorizationCode result = await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> _);

            Assert.That(result, Is.EqualTo(authorizationCode));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_ReturnsClaimCollectionNotEqualToNullInOutValue()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut);
            await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> claims);

            Assert.That(claims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_ReturnsNonEmptyClaimCollectionInOutValue()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut);
            await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> claims);

            Assert.That(claims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToAuthorizationCodeAsync_WhenCalled_ReturnsNonEmptyClaimCollectionMatchingClaimCollectionFromKeyValueEntryInOutValue()
        {
            IAuthorizationDataConverter sut = CreateSut();

            Claim[] claimsCollection = CreateClaims().ToArray();
            IKeyValueEntry keyValueEntry = await CreateKeyValueEntryAsync(sut, claims: claimsCollection);
            await sut.ToAuthorizationCodeAsync(keyValueEntry, out IEnumerable<Claim> claims);

            Assert.That(claimsCollection.All(claim => claims.Any(c => c.Type == claim.Type && c.Value == claim.Value && c.ValueType == claim.ValueType && c.Issuer == claim.Issuer)), Is.True);
        }

        private IAuthorizationDataConverter CreateSut(IAuthorizationCode authorizationCode = null)
        {
            _authorizationCodeFactoryMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                .Returns(_authorizationCodeBuilderMock.Object);

            _authorizationCodeBuilderMock.Setup(m => m.Build())
                .Returns(authorizationCode ?? _fixture.BuildAuthorizationCodeMock().Object);

            return new BusinessLogic.Security.Logic.AuthorizationDataConverter(_authorizationCodeFactoryMock.Object);
        }

        protected Task<IKeyValueEntry> CreateKeyValueEntryAsync(IAuthorizationDataConverter authorizationDataConverter, IAuthorizationCode authorizationCode = null, IEnumerable<Claim> claims = null)
        {
            NullGuard.NotNull(authorizationDataConverter, nameof(authorizationDataConverter));

            return authorizationDataConverter.ToKeyValueEntryAsync(authorizationCode ?? Fixture.BuildAuthorizationCodeMock().Object, claims ?? CreateClaims());
        }
    }
}