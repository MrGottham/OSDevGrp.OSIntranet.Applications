using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.UpdateClientSecretIdentityCommand
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
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsNotNull()
        {
            IUpdateClientSecretIdentityCommand sut = CreateSut();

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithoutClientIdAndClientSecret_ReturnsClientSecretIdentityWhereIdentifierIsEqualToIdentifierFromUpdateClientSecretIdentityCommand()
        {
            int identifier = _fixture.Create<int>();
            IUpdateClientSecretIdentityCommand sut = CreateSut(identifier: identifier);

            IClientSecretIdentity result = sut.ToDomain();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsNotNull()
        {
            IUpdateClientSecretIdentityCommand sut = CreateSut();

            IClientSecretIdentity result = sut.ToDomain(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledWithClientIdAndClientSecret_ReturnsClientSecretIdentityWhereIdentifierIsEqualToIdentifierFromUpdateClientSecretIdentityCommand()
        {
            int identifier = _fixture.Create<int>();
            IUpdateClientSecretIdentityCommand sut = CreateSut(identifier: identifier);

            IClientSecretIdentity result = sut.ToDomain(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        private IUpdateClientSecretIdentityCommand CreateSut(int? identifier = null)
        {
            return _fixture.Build<BusinessLogic.Security.Commands.UpdateClientSecretIdentityCommand>()
                .With(m => m.Identifier, identifier ?? _fixture.Create<int>())
                .With(m => m.Claims, _fixture.CreateClaims(_random))
                .Create();
        }
    }
}