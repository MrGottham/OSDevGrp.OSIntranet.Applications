﻿using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.Validator
{
    [TestFixture]
    public class ObjectTests
    {
        #region Private variables

        private Mock<IIntegerValidator> _integerValidatorMock;
        private Mock<IDecimalValidator> _decimalValidatorMock;
        private Mock<IStringValidator> _stringValidatorMock;
        private Mock<IDateTimeValidator> _dateTimeValidatorMock;
        private Mock<IObjectValidator> _objectValidatorMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _integerValidatorMock = new Mock<IIntegerValidator>();
            _decimalValidatorMock = new Mock<IDecimalValidator>();
            _stringValidatorMock = new Mock<IStringValidator>();
            _dateTimeValidatorMock = new Mock<IDateTimeValidator>();
            _objectValidatorMock = new Mock<IObjectValidator>();
        }

        [Test]
        [Category("UnitTest")]
        public void Object_WhenCalled_AssertObjectValidatorFromConstructor()
        {
            IValidator sut = CreateSut();

            IObjectValidator objectValidator = sut.Object;

            Assert.That(objectValidator, Is.EqualTo(_objectValidatorMock.Object));
        }

        private IValidator CreateSut()
        {
            return new BusinessLogic.Validation.Validator(_integerValidatorMock.Object, _decimalValidatorMock.Object, _stringValidatorMock.Object, _dateTimeValidatorMock.Object, _objectValidatorMock.Object);
        }
    }
}
