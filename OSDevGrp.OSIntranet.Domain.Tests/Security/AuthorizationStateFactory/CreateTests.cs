using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateFactory
{
    [TestFixture]
    public class CreateTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenResponseTypeIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, _fixture.Create<string>(), CreateRedirectUri(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("responseType"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenResponseTypeIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(string.Empty, _fixture.Create<string>(), CreateRedirectUri(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("responseType"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenResponseTypeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(" ", _fixture.Create<string>(), CreateRedirectUri(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("responseType"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenClientIdIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), null, CreateRedirectUri(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenClientIdIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), string.Empty, CreateRedirectUri(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenClientIdIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), " ", CreateRedirectUri(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenRedirectUriIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), _fixture.Create<string>(), null, CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("redirectUri"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenScopesIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), _fixture.Create<string>(), CreateRedirectUri(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("scopes"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationStateBuilder result = sut.Create(_fixture.Create<string>(), _fixture.Create<string>(), CreateRedirectUri(), CreateScopes());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsAuthorizationStateBuilder()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationStateBuilder result = sut.Create(_fixture.Create<string>(), _fixture.Create<string>(), CreateRedirectUri(), CreateScopes());

            Assert.That(result, Is.TypeOf<Domain.Security.AuthorizationStateBuilder>());
        }

        private IAuthorizationStateFactory CreateSut()
        {
            return new Domain.Security.AuthorizationStateFactory();
        }

        private Uri CreateRedirectUri()
        {
            return new Uri($"https://{_fixture.Create<string>().Replace("/", string.Empty)}.local/{_fixture.Create<string>().Replace("/", string.Empty)}", UriKind.Absolute);
        }

        private string[] CreateScopes()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
        }
    }
}