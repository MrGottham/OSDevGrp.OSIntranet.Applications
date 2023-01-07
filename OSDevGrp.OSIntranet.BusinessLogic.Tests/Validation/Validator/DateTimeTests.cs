using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.Validator
{
    [TestFixture]
    public class DateTimeTests
    {
        #region Private variables

        private Mock<IIntegerValidator> _integerValidatorMock;
        private Mock<IDecimalValidator> _decimalValidatorMock;
        private Mock<IStringValidator> _stringValidatorMock;
        private Mock<IDateTimeValidator> _dateTimeValidatorMock;
        private Mock<IObjectValidator> _objectValidatorMock;
        private Mock<IEnumerableValidator> _enumerableValidatorMock;
        private Mock<IPermissionValidator> _permissionValidatorMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _integerValidatorMock = new Mock<IIntegerValidator>();
            _decimalValidatorMock = new Mock<IDecimalValidator>();
            _stringValidatorMock = new Mock<IStringValidator>();
            _dateTimeValidatorMock = new Mock<IDateTimeValidator>();
            _objectValidatorMock = new Mock<IObjectValidator>();
            _enumerableValidatorMock = new Mock<IEnumerableValidator>();
            _permissionValidatorMock = new Mock<IPermissionValidator>();
        }

        [Test]
        [Category("UnitTest")]
        public void DateTime_WhenCalled_AssertDateTimeValidatorFromConstructor()
        {
            IValidator sut = CreateSut();

            IDateTimeValidator dateTimeValidator = sut.DateTime;

            Assert.That(dateTimeValidator, Is.EqualTo(_dateTimeValidatorMock.Object));
        }

        private IValidator CreateSut()
        {
            return new BusinessLogic.Validation.Validator(_integerValidatorMock.Object, _decimalValidatorMock.Object, _stringValidatorMock.Object, _dateTimeValidatorMock.Object, _objectValidatorMock.Object, _enumerableValidatorMock.Object, _permissionValidatorMock.Object);
        }
    }
}