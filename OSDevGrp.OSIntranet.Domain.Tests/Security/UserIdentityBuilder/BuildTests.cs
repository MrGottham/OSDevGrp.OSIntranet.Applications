﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentityBuilder
{
    [TestFixture]
    public class BuildTests : UserIdentityBuilderTestBase
    {
        #region Private variables

        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _random = new Random();

            Fixture = new Fixture();
            Fixture.Customize<Claim>(builder => builder.FromFactory(() => new Claim(Fixture.Create<string>(), Fixture.Create<string>())));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_AssertCorrectExternalUserIdentifier()
        {
            string externalUserIdentifier = Fixture.Create<string>();
            IUserIdentityBuilder sut = CreateSut(externalUserIdentifier);

            IUserIdentity result = sut.Build();

            Assert.That(result.ExternalUserIdentifier, Is.EqualTo(externalUserIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithIdentifierHasNotBeenCalled_AssertDefaultIdentifier()
        {
            IUserIdentityBuilder sut = CreateSut();

            IUserIdentity result = sut.Build();

            Assert.That(result.Identifier, Is.EqualTo(default(int)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithIdentifierHasBeenCalled_AssertCorrectIdentifier()
        {
            IUserIdentityBuilder sut = CreateSut();

            int identifier = Fixture.Create<int>();
            IUserIdentity result = sut.WithIdentifier(identifier).Build();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereAddClaimsHasNotBeenCalled_AssertDefaultClaims()
        {
            IUserIdentityBuilder sut = CreateSut();

            IUserIdentity result = sut.Build();

            Assert.That(result.ToClaimsIdentity().Claims.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereAddClaimsHasBeenCalled_AssertCorrectClaims()
        {
            IUserIdentityBuilder sut = CreateSut();

            IEnumerable<Claim> claimCollection = Fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();
            IUserIdentity result = sut.AddClaims(claimCollection).Build();

            Assert.That(result.ToClaimsIdentity().Claims.Count(), Is.EqualTo(1 + claimCollection.Count()));
            foreach (Claim claim in claimCollection)
            {
                Assert.That(result.ToClaimsIdentity().Claims.SingleOrDefault(m => string.Compare(m.Type, claim.Type, StringComparison.Ordinal) == 0), Is.Not.Null);
            }
        }
    }
}
