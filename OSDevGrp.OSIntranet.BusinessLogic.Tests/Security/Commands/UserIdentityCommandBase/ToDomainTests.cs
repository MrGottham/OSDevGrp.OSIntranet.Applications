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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.UserIdentityCommandBase
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
        public void ToDomain_WhenCalled_ReturnsNotNull(bool mapIdentifier)
        {
	        IUserIdentityCommand sut = CreateSut(mapIdentifier: mapIdentifier);

	        IUserIdentity result = sut.ToDomain();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenIdentifierShouldBeMapped_ReturnsUserIdentityWithIdentifierFromCommand()
        {
            int identifier = _fixture.Create<int>();
            IUserIdentityCommand sut = CreateSut(identifier, mapIdentifier: true);

            IUserIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenIdentifierShouldNotBeMapped_ReturnsUserIdentityWithIdentifierEqualToZero()
        {
            IUserIdentityCommand sut = CreateSut(_fixture.Create<int>(), mapIdentifier: false);

            IUserIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_ReturnsUserIdentityWithExternalUserIdentifierFromCommand(bool mapIdentifier)
        {
            string externalUserIdentifier = _fixture.Create<string>();
            IUserIdentityCommand sut = CreateSut(externalUserIdentifier: externalUserIdentifier, mapIdentifier: mapIdentifier);

            IUserIdentity result = sut.ToDomain();

            Assert.That(result.ExternalUserIdentifier, Is.EqualTo(externalUserIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_ReturnsUserIdentityWithClaimsFromCommand(bool mapIdentifier)
        {
            IEnumerable<Claim> claims = _fixture.CreateClaims(_random);
            IUserIdentityCommand sut = CreateSut(claims: claims, mapIdentifier: mapIdentifier);

            IUserIdentity result = sut.ToDomain();

            foreach (Claim claim in claims)
            {
                Assert.That(result.ToClaimsIdentity().Claims.SingleOrDefault(m => string.CompareOrdinal(claim.ValueType, m.ValueType) == 0 && string.CompareOrdinal(claim.Value, m.Value) == 0), Is.Not.Null);
            }
        }

        private IUserIdentityCommand CreateSut(int? identifier = null, string externalUserIdentifier = null, IEnumerable<Claim> claims = null, bool? mapIdentifier = null)
        {
            return new Sut(identifier ?? _fixture.Create<int>(), externalUserIdentifier ?? _fixture.Create<string>(), claims ?? _fixture.CreateClaims(_random), mapIdentifier ?? _fixture.Create<bool>());
        }

        private class Sut : BusinessLogic.Security.Commands.UserIdentityCommandBase
        {
            #region Constructor

            public Sut(int identifier, string externalUserIdentifier, IEnumerable<Claim> claims, bool mapIdentifier)
            {
                NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier))
                    .NotNull(claims, nameof(claims));

                Identifier = identifier;
                ExternalUserIdentifier = externalUserIdentifier;
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