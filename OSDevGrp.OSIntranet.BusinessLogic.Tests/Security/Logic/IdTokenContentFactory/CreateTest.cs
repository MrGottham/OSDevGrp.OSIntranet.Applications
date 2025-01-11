using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentFactory
{
    [TestFixture]
    public class CreateTest
    {
        #region Private variables

        private Mock<IClaimsSelector> _claimsSelectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _claimsSelectorMock = new Mock<IClaimsSelector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenNameIdentifierIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, CreateUserInfo(), CreateAuthenticationTime(), CreateSupportedScopes(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nameIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenNameIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(string.Empty, CreateUserInfo(), CreateAuthenticationTime(), CreateSupportedScopes(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nameIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenNameIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(" ", CreateUserInfo(), CreateAuthenticationTime(), CreateSupportedScopes(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nameIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenUserInfoIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), null, CreateAuthenticationTime(), CreateSupportedScopes(), CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("userInfo"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenSupportedScopesIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), CreateUserInfo(), CreateAuthenticationTime(), null, CreateScopes()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("supportedScopes"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenScopesIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), CreateUserInfo(), CreateAuthenticationTime(), CreateSupportedScopes(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("scopes"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsNotNull()
        {
            IIdTokenContentFactory sut = CreateSut();

            IIdTokenContentBuilder result = sut.Create(_fixture.Create<string>(), CreateUserInfo(), CreateAuthenticationTime(), CreateSupportedScopes(), CreateScopes());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsIdTokenContentBuilder()
        {
            IIdTokenContentFactory sut = CreateSut();

            IIdTokenContentBuilder result = sut.Create(_fixture.Create<string>(), CreateUserInfo(), CreateAuthenticationTime(), CreateSupportedScopes(), CreateScopes());

            Assert.That(result, Is.TypeOf<BusinessLogic.Security.Logic.IdTokenContentBuilder>());
        }

        private IIdTokenContentFactory CreateSut()
        {
            return new BusinessLogic.Security.Logic.IdTokenContentFactory(_claimsSelectorMock.Object);
        }

        private IUserInfo CreateUserInfo()
        {
            return _fixture.BuildUserInfoMock().Object;
        }

        private DateTimeOffset CreateAuthenticationTime()
        {
            return DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1);
        }

        private IReadOnlyDictionary<string, IScope> CreateSupportedScopes()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10))
                .ToDictionary(supportedScope => supportedScope, supportedScope => _fixture.BuildScopeMock(name: supportedScope).Object)
                .AsReadOnly();
        }

        private IReadOnlyCollection<string> CreateScopes()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
        }
    }
}