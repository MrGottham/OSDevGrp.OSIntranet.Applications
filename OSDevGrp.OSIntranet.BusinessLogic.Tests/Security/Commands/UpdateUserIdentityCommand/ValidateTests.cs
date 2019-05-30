using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.UpdateUserIdentityCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IUpdateUserIdentityCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _securityRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenSecurityRepositoryIsNull_ThrowsArgumentNullException()
        {
            IUpdateUserIdentityCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("securityRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidator()
        {
            int identifier = _fixture.Create<int>();
            IUpdateUserIdentityCommand sut = CreateSut(identifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == identifier), It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(value => sut.GetType() == value),
                    It.Is<string>(value => string.Compare(value, "Identifier", StringComparison.Ordinal) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IUpdateUserIdentityCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IUpdateUserIdentityCommand CreateSut(int? identifier = null)
        {
            return _fixture.Build<OSDevGrp.OSIntranet.BusinessLogic.Security.Commands.UpdateUserIdentityCommand>()
                .With(m => m.Identifier, identifier ?? _fixture.Create<int>())
                .With(m => m.Claims, new List<Claim>())
                .Create();
        }
    }
}