using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.InfoCommandBase
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
            IInfoCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithYear()
        {
            short year = _fixture.Create<short>();
            IInfoCommand sut = CreateSut(year);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == year),
                    It.Is<int>(minValue => minValue == DateTime.Today.Year),
                    It.Is<int>(maxValue => maxValue == InfoBase<IInfo>.MaxYear),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Year") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMonth()
        {
            short month = _fixture.Create<short>();
            IInfoCommand sut = CreateSut(month: month);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == month),
                    It.Is<int>(minValue => minValue == InfoBase<IInfo>.MinMonth),
                    It.Is<int>(maxValue => maxValue == InfoBase<IInfo>.MaxMonth),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Month") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithYearAndMonth()
        {
            short year = _fixture.Create<short>();
            short month = _fixture.Create<short>();
            IInfoCommand sut = CreateSut(year, month);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == year * 100 + month),
                    It.Is<int>(minValue => minValue == DateTime.Today.Year * 100 + DateTime.Today.Month),
                    It.Is<int>(maxValue => maxValue == InfoBase<IInfo>.MaxYear * 100 + InfoBase<IInfo>.MaxMonth),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Year,Month") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IInfoCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IInfoCommand CreateSut(short? year = null, short? month = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Year, year ?? _fixture.Create<short>())
                .With(m => m.Month, month ?? _fixture.Create<short>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.InfoCommandBase
        {
        }
    }
}