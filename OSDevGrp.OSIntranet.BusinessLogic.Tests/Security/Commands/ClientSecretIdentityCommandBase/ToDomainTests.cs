using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsNotNull(bool mapIdentifier)
        {
	        IClientSecretIdentityCommand sut = CreateSut();

	        IClientSecretIdentity result = sut.ToDomain();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecretAndIdentifierShouldBeMapped_ReturnsClientSecretIdentityWithIdentifierFromCommand()
        {
            int identifier = _fixture.Create<int>();
            IClientSecretIdentityCommand sut = CreateSut(identifier, mapIdentifier: true);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecretAndIdentifierShouldNotBeMapped_ReturnsClientSecretIdentityWithIdentifierEqualToZero()
        {
            IClientSecretIdentityCommand sut = CreateSut(_fixture.Create<int>(), mapIdentifier: false);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWithFriendlyNameFromCommand(bool mapIdentifier)
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result.FriendlyName, Is.EqualTo(friendlyName));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWhereClientIdIsNotNullEmptyOrWhiteSpace(bool mapIdentifier)
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(string.IsNullOrWhiteSpace(result.ClientId), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWhereClientSecretIsNotNullEmptyOrWhiteSpace(bool mapIdentifier)
        {
            string friendlyName = _fixture.Create<string>();
            IClientSecretIdentityCommand sut = CreateSut(friendlyName: friendlyName);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(string.IsNullOrWhiteSpace(result.ClientSecret), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWithClaimsFromCommand(bool mapIdentifier)
        {
            IEnumerable<Claim> claims = _fixture.CreateClaims(_random);
            IClientSecretIdentityCommand sut = CreateSut(claims: claims);

            IClientSecretIdentity result = sut.ToDomain();

            foreach (Claim claim in claims)
            {
                Assert.That(result.ToClaimsIdentity().Claims.SingleOrDefault(m => string.CompareOrdinal(claim.ValueType, m.ValueType) == 0 && string.CompareOrdinal(claim.Value, m.Value) == 0), Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenClientIdIsNull_ThrowsArgumentNullException(bool mapIdentifier)
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null, clientSecret));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenClientIdIsEmpty_ThrowsArgumentNullException(bool mapIdentifier)
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(string.Empty, clientSecret));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenClientIdIsWhiteSpace_ThrowsArgumentNullException(bool mapIdentifier)
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(" ", clientSecret));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenClientSecretIsNull_ThrowsArgumentNullException(bool mapIdentifier)
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientId = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(clientId, null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenClientSecretIsEmpty_ThrowsArgumentNullException(bool mapIdentifier)
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientId = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(clientId, string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenClientSecretIsWhiteSpace_ThrowsArgumentNullException(bool mapIdentifier)
        {
            IClientSecretIdentityCommand sut = CreateSut();

            string clientId = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(clientId, " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsNotNull(bool mapIdentifier)
        {
	        IClientSecretIdentityCommand sut = CreateSut();

	        string clientId = _fixture.Create<string>();
	        string clientSecret = _fixture.Create<string>();
	        IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecretAndIdentifierShouldBeMapped_ReturnsClientSecretIdentityWithIdentifierFromCommand()
        {
            int identifier = _fixture.Create<int>();
            IClientSecretIdentityCommand sut = CreateSut(identifier, mapIdentifier: true);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecretAndIdentifierShouldNotBeMapped_ReturnsClientSecretIdentityWithIdentifierEqualToZero()
        {
            IClientSecretIdentityCommand sut = CreateSut(_fixture.Create<int>(), mapIdentifier: false);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            Assert.That(result.Identifier, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWithFriendlyNameFromCommand(bool mapIdentifier)
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
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWithClientId(bool mapIdentifier)
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
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWithClientSecret(bool mapIdentifier)
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
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWithClaimsFromCommand(bool mapIdentifier)
        {
            IEnumerable<Claim> claims = _fixture.CreateClaims(_random);
            IClientSecretIdentityCommand sut = CreateSut(claims: claims);

            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity result = sut.ToDomain(clientId, clientSecret);

            foreach (Claim claim in claims)
            {
                Assert.That(result.ToClaimsIdentity().Claims.SingleOrDefault(m => string.CompareOrdinal(claim.ValueType, m.ValueType) == 0 && string.CompareOrdinal(claim.Value, m.Value) == 0), Is.Not.Null);
            }
        }

        private IClientSecretIdentityCommand CreateSut(int? identifier = null, string friendlyName = null, IEnumerable<Claim> claims = null, bool? mapIdentifier = null)
        {
            return new Sut(identifier ?? _fixture.Create<int>(), friendlyName ?? _fixture.Create<string>(), claims ?? _fixture.CreateClaims(_random), mapIdentifier ?? _fixture.Create<bool>());
        }

        private class Sut : BusinessLogic.Security.Commands.ClientSecretIdentityCommandBase
        {
            #region Constructor

            public Sut(int identifier, string friendlyName, IEnumerable<Claim> claims, bool mapIdentifier)
            {
                NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName))
                    .NotNull(claims, nameof(claims));

                Identifier = identifier;
                FriendlyName = friendlyName;
                Claims = claims;
                MapIdentifier = mapIdentifier;
            }

            #endregion

            #region Properties

            protected override bool MapIdentifier { get; }

            #endregion
        }
    }
}