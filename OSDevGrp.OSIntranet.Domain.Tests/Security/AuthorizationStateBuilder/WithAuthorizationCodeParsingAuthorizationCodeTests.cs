using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateBuilder
{
    [TestFixture]
    public class WithAuthorizationCodeParsingAuthorizationCodeTests : AuthorizationStateBuilderTestBase
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
        public void WithAuthorizationCode_WhenAuthorizationCodeIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthorizationCode(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizationCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizationCode_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithAuthorizationCode(_fixture.BuildAuthorizationCodeMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizationCode_WhenCalled_ReturnsSameAuthorizationStateBuilder()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithAuthorizationCode(_fixture.BuildAuthorizationCodeMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        private IAuthorizationStateBuilder CreateSut()
        {
            return CreateSut(_fixture, _random);
        }
    }
}