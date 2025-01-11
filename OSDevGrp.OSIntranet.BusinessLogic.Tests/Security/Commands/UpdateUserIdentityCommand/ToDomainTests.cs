using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.UpdateUserIdentityCommand
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
            IUpdateUserIdentityCommand sut = CreateSut();

            IUserIdentity result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsUserIdentityWhereIdentifierIsEqualToIdentifierFromUpdateUserIdentityCommand()
        {
            int identifier = _fixture.Create<int>();
            IUpdateUserIdentityCommand sut = CreateSut(identifier: identifier);

            IUserIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        private IUpdateUserIdentityCommand CreateSut(int? identifier = null)
        {
            return _fixture.Build<BusinessLogic.Security.Commands.UpdateUserIdentityCommand>()
                .With(m => m.Identifier, identifier ?? _fixture.Create<int>())
                .With(m => m.Claims, _fixture.CreateClaims(_random))
                .Create();
        }
    }
}