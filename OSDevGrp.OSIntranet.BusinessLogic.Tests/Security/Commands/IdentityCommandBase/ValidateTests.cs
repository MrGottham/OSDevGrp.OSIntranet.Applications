using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.IdentityCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _securityRepositoryMock = new Mock<ISecurityRepository>();

            _fixture = new Fixture();
            _fixture.Customize<Claim>(builder => builder.FromFactory(() => new Claim(_fixture.Create<string>(), _fixture.Create<string>())));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IIdentityCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _securityRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenSecurityRepositoryIsNull_ThrowsArgumentNullException()
        {
            IIdentityCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("securityRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidator()
        {
            IEnumerable<Claim> claims = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();
            IIdentityCommand sut = CreateSut(claims);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<IEnumerable<Claim>>(value => value == claims),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Claims", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IIdentityCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IIdentityCommand CreateSut(IEnumerable<Claim> claims = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Claims, claims ?? _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList())
                .Create();
        }

        private class Sut : OSDevGrp.OSIntranet.BusinessLogic.Security.Commands.IdentityCommandBase
        {
        }
    }
}