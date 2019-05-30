using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.ClientSecretIdentityCommandBase
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<Claim>(builder => builder.FromFactory(() => new Claim(_fixture.Create<string>(), _fixture.Create<string>())));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentity()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<ClientSecretIdentity>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWithIdentifierFromCommand()
        {
            int identifier = _fixture.Create<int>();
            IClientSecretIdentityCommand sut = CreateSut(identifier);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWithFriendlyNameFromCommand()
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result.FriendlyName, Is.EqualTo(friendlyName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWhereClientIdIsNotNullEmptyOrWhiteSpace()
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(string.IsNullOrWhiteSpace(result.ClientId), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWhereClientSecretIsNotNullEmptyOrWhiteSpace()
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(string.IsNullOrWhiteSpace(result.ClientSecret), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWithClaimsFromCommand()
        {
            IEnumerable<Claim> claims = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();;
            IClientSecretIdentityCommand sut = CreateSut(claims: claims);

            IClientSecretIdentity result = sut.ToDomain();

            foreach (Claim claim in claims)
            {
                Assert.That(result.ToClaimsIdentity().Claims.SingleOrDefault(m => string.CompareOrdinal(claim.ValueType, m.ValueType) == 0 && string.CompareOrdinal(claim.Value, m.Value) == 0), Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenClientIdIsNull_ThrowsArgumentNullException()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null, clientSecret));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenClientIdIsEmpty_ThrowsArgumentNullException()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(string.Empty, clientSecret));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenClientIdIsWhiteSpace_ThrowsArgumentNullException()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(" ", clientSecret));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenClientSecretIsNull_ThrowsArgumentNullException()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientId = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(clientId, null));

            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenClientSecretIsEmpty_ThrowsArgumentNullException()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientId = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(clientId, string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenClientSecretIsWhiteSpace_ThrowsArgumentNullException()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientId = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(clientId, " "));

            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentity()
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            Assert.That(result, Is.TypeOf<ClientSecretIdentity>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWithIdentifierFromCommand()
        {
            int identifier = _fixture.Create<int>();
            IClientSecretIdentityCommand sut = CreateSut(identifier);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWithFriendlyNameFromCommand()
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            Assert.That(result.FriendlyName, Is.EqualTo(friendlyName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWithClientId()
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            Assert.That(result.ClientId, Is.EqualTo(clientId));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWithClientSecret()
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            Assert.That(result.ClientSecret, Is.EqualTo(clientSecret));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWithClaimsFromCommand()
        {
            IEnumerable<Claim> claims = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();;
            IClientSecretIdentityCommand sut = CreateSut(claims: claims);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            foreach (Claim claim in claims)
            {
                Assert.That(result.ToClaimsIdentity().Claims.SingleOrDefault(m => string.CompareOrdinal(claim.ValueType, m.ValueType) == 0 && string.CompareOrdinal(claim.Value, m.Value) == 0), Is.Not.Null);
            }
        }

        private IClientSecretIdentityCommand CreateSut(int? identifier = null, string friendlyName = null, IEnumerable<Claim> claims = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Identifier, identifier ?? _fixture.Create<int>())
                .With(m => m.FriendlyName, friendlyName ?? _fixture.Create<string>())
                .With(m => m.Claims, claims ?? _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList())
                .Create();
        }

        private class Sut : OSDevGrp.OSIntranet.BusinessLogic.Security.Commands.ClientSecretIdentityCommandBase
        {
        }
    }
}