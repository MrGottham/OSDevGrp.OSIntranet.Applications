using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ScopeBuilder
{
    [TestFixture]
    public class BuildTests : ScopeBuilderTestBase
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
        public void Build_WhenCalled_ReturnsNotNull()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsScope()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result, Is.TypeOf<Domain.Security.Scope>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsScopeWhereNameIsNotNull()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsScopeWhereNameIsNotEmpty()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsScopeWhereNameIsEqualToNameFromConstructor()
        {
            string name = _fixture.Create<string>();
            IScopeBuilder sut = CreateSut(_fixture, name: name);

            IScope result = sut.Build();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsScopeWhereDescriptionIsNotNull()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result.Description, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsScopeWhereDescriptionIsNotEmpty()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result.Description, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsScopeWhereDescriptionIsEqualToDescriptionFromConstructor()
        {
            string description = _fixture.Create<string>();
            IScopeBuilder sut = CreateSut(_fixture, description: description);

            IScope result = sut.Build();

            Assert.That(result.Description, Is.EqualTo(description));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasNotBeenCalled_ReturnsScopeWhereRelatedClaimsIsNotNull()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result.RelatedClaims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasNotBeenCalled_ReturnsScopeWhereRelatedClaimsIsEmpty()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.Build();

            Assert.That(result.RelatedClaims, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalled_ReturnsScopeWhereRelatedClaimsIsNotNull()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.WithRelatedClaim(_fixture.Create<string>()).Build();

            Assert.That(result.RelatedClaims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalled_ReturnsScopeWhereRelatedClaimsIsNotEmpty()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.WithRelatedClaim(_fixture.Create<string>()).Build();

            Assert.That(result.RelatedClaims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalled_ReturnsScopeWhereRelatedClaimsContainsOneRelatedClaim()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.WithRelatedClaim(_fixture.Create<string>()).Build();

            Assert.That(result.RelatedClaims.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalled_ReturnsScopeWhereRelatedClaimsContainsRelatedClaim()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            string claimType = _fixture.Create<string>();
            IScope result = sut.WithRelatedClaim(claimType).Build();

            Assert.That(result.RelatedClaims.Contains(claimType), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalledMultipleTimes_ReturnsScopeWhereRelatedClaimsIsNotNull()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.WithRelatedClaim(_fixture.Create<string>())
                .WithRelatedClaim(_fixture.Create<string>())
                .WithRelatedClaim(_fixture.Create<string>())
                .Build();

            Assert.That(result.RelatedClaims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalledMultipleTimes_ReturnsScopeWhereRelatedClaimsIsNotEmpty()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScope result = sut.WithRelatedClaim(_fixture.Create<string>())
                .WithRelatedClaim(_fixture.Create<string>())
                .WithRelatedClaim(_fixture.Create<string>())
                .Build();

            Assert.That(result.RelatedClaims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalledMultipleTimes_ReturnsScopeWhereRelatedClaimsContainsMatchingNumberOfRelatedClaims()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            HashSet<string> claimTypes = [.._fixture.CreateMany<string>(_random.Next(3, 10)).ToArray()];
            foreach (string claimType in claimTypes)
            {
                sut = sut.WithRelatedClaim(claimType);
            }

            IScope result = sut.Build();

            Assert.That(result.RelatedClaims.Count(), Is.EqualTo(claimTypes.Count));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRelatedClaimHasBeenCalledMultipleTimes_ReturnsScopeWhereRelatedClaimsContainsEachRelatedClaim()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            HashSet<string> claimTypes = [.._fixture.CreateMany<string>(_random.Next(3, 10)).ToArray()];
            foreach (string claimType in claimTypes)
            {
                sut = sut.WithRelatedClaim(claimType);
            }

            IScope result = sut.Build();

            Assert.That(claimTypes.All(claimType => result.RelatedClaims.Contains(claimType)), Is.True);
        }
    }
}