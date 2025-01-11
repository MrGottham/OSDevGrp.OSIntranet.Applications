using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.CreateUserIdentityCommand
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
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
            ICreateUserIdentityCommand sut = CreateSut();

            IUserIdentity result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsUserIdentityWhereIdentifierIsEqualToZero()
        {
            ICreateUserIdentityCommand sut = CreateSut();

            IUserIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(0));
        }

        private ICreateUserIdentityCommand CreateSut(int? identifier = null)
        {
            return _fixture.Build<BusinessLogic.Security.Commands.CreateUserIdentityCommand>()
                .With(m => m.Identifier, identifier ?? _fixture.Create<int>())
                .With(m => m.Claims, _fixture.CreateClaims(_random))
                .Create();
        }
    }
}