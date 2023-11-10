using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
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
            _fixture.Customize<Claim>(builder => builder.FromFactory(() => new Claim(_fixture.Create<string>(), _fixture.Create<string>())));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
	        IUserIdentityCommand sut = CreateSut();

	        IUserIdentity result = sut.ToDomain();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsUserIdentityWithIdentifierFromCommand()
        {
            int identifier = _fixture.Create<int>();
            IUserIdentityCommand sut = CreateSut(identifier);

            IUserIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsUserIdentityWithExternalUserIdentifierFromCommand()
        {
            string externalUserIdentifier = _fixture.Create<string>();
            IUserIdentityCommand sut = CreateSut(externalUserIdentifier: externalUserIdentifier);

            IUserIdentity result = sut.ToDomain();

            Assert.That(result.ExternalUserIdentifier, Is.EqualTo(externalUserIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsUserIdentityWithClaimsFromCommand()
        {
            IEnumerable<Claim> claims = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();
            IUserIdentityCommand sut = CreateSut(claims: claims);

            IUserIdentity result = sut.ToDomain();

            foreach (Claim claim in claims)
            {
                Assert.That(result.ToClaimsIdentity().Claims.SingleOrDefault(m => string.CompareOrdinal(claim.ValueType, m.ValueType) == 0 && string.CompareOrdinal(claim.Value, m.Value) == 0), Is.Not.Null);
            }
        }

        private IUserIdentityCommand CreateSut(int? identifier = null, string externalUserIdentifier = null, IEnumerable<Claim> claims = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Identifier, identifier ?? _fixture.Create<int>())
                .With(m => m.ExternalUserIdentifier, externalUserIdentifier ?? _fixture.Create<string>())
                .With(m => m.Claims, claims ?? _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList())
                .Create();
        }

        private class Sut : BusinessLogic.Security.Commands.UserIdentityCommandBase
        {
        }
    }
}