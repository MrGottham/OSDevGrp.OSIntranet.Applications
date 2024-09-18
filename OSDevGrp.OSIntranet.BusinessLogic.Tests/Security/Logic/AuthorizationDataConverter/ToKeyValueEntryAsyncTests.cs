using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthorizationDataConverter
{
    [TestFixture]
    public class ToKeyValueEntryAsyncTests : AuthorizationDataConverterTestBase
    {
        #region Private variables

        private Mock<IAuthorizationCodeFactory> _authorizationCodeFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        [SetUp]
        public void SetUp()
        {
            _authorizationCodeFactoryMock = new Mock<IAuthorizationCodeFactory>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToKeyValueEntryAsync_WhenAuthorizationCodeIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationDataConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToKeyValueEntryAsync(null, Fixture.CreateClaims(Random), CreateAuthorizationData()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizationCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToKeyValueEntryAsync_WhenClaimsIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationDataConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToKeyValueEntryAsync(_fixture.BuildAuthorizationCodeMock().Object, null, CreateAuthorizationData()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claims"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToKeyValueEntryAsync_WhenAuthorizationDataIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationDataConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToKeyValueEntryAsync(_fixture.BuildAuthorizationCodeMock().Object, Fixture.CreateClaims(Random), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizationData"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_AssertValueWasCalledOnAuthorizationCode()
        {
            IAuthorizationDataConverter sut = CreateSut();

            Mock<IAuthorizationCode> authorizationCodeMock = _fixture.BuildAuthorizationCodeMock();
            await sut.ToKeyValueEntryAsync(authorizationCodeMock.Object, Fixture.CreateClaims(Random), CreateAuthorizationData());

            authorizationCodeMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_AssertExpiresWasCalledOnAuthorizationCode()
        {
            IAuthorizationDataConverter sut = CreateSut();

            Mock<IAuthorizationCode> authorizationCodeMock = _fixture.BuildAuthorizationCodeMock();
            await sut.ToKeyValueEntryAsync(authorizationCodeMock.Object, Fixture.CreateClaims(Random), CreateAuthorizationData());

            authorizationCodeMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IKeyValueEntry keyValueEntry = await sut.ToKeyValueEntryAsync(authorizationCode, Fixture.CreateClaims(Random), CreateAuthorizationData());

            Assert.That(keyValueEntry, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_ReturnsKeyValueEntry()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IKeyValueEntry keyValueEntry = await sut.ToKeyValueEntryAsync(authorizationCode, Fixture.CreateClaims(Random), CreateAuthorizationData());

            Assert.That(keyValueEntry, Is.TypeOf<KeyValueEntry>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_ReturnsKeyValueEntryWhereKeyIsNotNull()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IKeyValueEntry keyValueEntry = await sut.ToKeyValueEntryAsync(authorizationCode, Fixture.CreateClaims(Random), CreateAuthorizationData());

            Assert.That(keyValueEntry.Key, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_ReturnsKeyValueEntryWhereKeyIsNotEmpty()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IKeyValueEntry keyValueEntry = await sut.ToKeyValueEntryAsync(authorizationCode, Fixture.CreateClaims(Random), CreateAuthorizationData());

            Assert.That(keyValueEntry.Key, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_ReturnsKeyValueEntryWhereKeyIsEqualToValueFromAuthorizationCode()
        {
            IAuthorizationDataConverter sut = CreateSut();

            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IKeyValueEntry keyValueEntry = await sut.ToKeyValueEntryAsync(authorizationCode, Fixture.CreateClaims(Random), CreateAuthorizationData());

            Assert.That(keyValueEntry.Key, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_ReturnsKeyValueEntryWhereValueIsNotNull()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IKeyValueEntry keyValueEntry = await sut.ToKeyValueEntryAsync(authorizationCode, Fixture.CreateClaims(Random), CreateAuthorizationData());

            Assert.That(keyValueEntry.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ToKeyValueEntryAsync_WhenCalled_ReturnsKeyValueEntryWhereValueIsNotEmpty()
        {
            IAuthorizationDataConverter sut = CreateSut();

            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IKeyValueEntry keyValueEntry = await sut.ToKeyValueEntryAsync(authorizationCode, Fixture.CreateClaims(Random), CreateAuthorizationData());

            Assert.That(keyValueEntry.Value, Is.Not.Empty);
        }

        private IAuthorizationDataConverter CreateSut()
        {
            return new BusinessLogic.Security.Logic.AuthorizationDataConverter(_authorizationCodeFactoryMock.Object);
        }
    }
}