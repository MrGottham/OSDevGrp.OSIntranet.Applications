using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.CreditInfoCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            ICreditInfoCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnDecimalValidatorWithCredit()
        {
            decimal credit = _fixture.Create<short>();
            ICreditInfoCommand sut = CreateSut(credit);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.DecimalValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<decimal>(value => value == credit),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Credit") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            ICreditInfoCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private ICreditInfoCommand CreateSut(decimal? credit = null)
        {
            return _fixture.Build<BusinessLogic.Accounting.Commands.CreditInfoCommand>()
                .With(m => m.Credit, credit ?? _fixture.Create<decimal>())
                .Create();
        }
    }
}