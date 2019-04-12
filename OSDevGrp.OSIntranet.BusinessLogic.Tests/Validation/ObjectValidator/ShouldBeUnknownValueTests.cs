using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.ObjectValidator
{
    [TestFixture]
    public class ShouldBeUnknownValueTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenIsUnknownValueGetterIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeUnknownValue(_fixture.Create<object>(), null, GetType(), _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("isUnknownValueGetter"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeUnknownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeUnknownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeUnknownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeUnknownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), " "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValueIsNullAndNullIsAllowed_ReturnsObjectValidator()
        {
            IObjectValidator sut = CreateSut();

            IValidator result = sut.ShouldBeUnknownValue<object>(null, async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), _fixture.Create<string>(), true);

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.ObjectValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeUnknownValue<object>(null, async obj => await Task.Run(() => _fixture.Create<bool>()), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValueIsNotNull_AssertIsUnknownValueGetterWasCalled()
        {
            IObjectValidator sut = CreateSut();

            object value = _fixture.Create<object>();
            object isUnknownValueGetterWasCalledWithObject = null;
            sut.ShouldBeUnknownValue(value, async obj => await Task.Run(() =>
                {
                    isUnknownValueGetterWasCalledWithObject = obj;
                    return true;
                }),
                GetType(),
                _fixture.Create<string>());

            Assert.That(isUnknownValueGetterWasCalledWithObject, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValueIsNotNullAndUnknown_ReturnsReturnsObjectValidator()
        {
            IObjectValidator sut = CreateSut();

            IValidator result = sut.ShouldBeUnknownValue(_fixture.Create<object>(), async obj => await Task.Run(() => true), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.ObjectValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValueIsNotNullAndKnown_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeUnknownValue(_fixture.Create<object>(), async obj => await Task.Run(() => false), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeUnknown));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeUnknownValue_WhenValueIsNotNullIsUnknownValueGetterThrowsException_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Exception exception = _fixture.Create<Exception>();
            bool IsUnknownValueGetter() => throw exception;

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeUnknownValue(_fixture.Create<object>(), async obj => await Task.Run((Func<bool>) IsUnknownValueGetter), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeUnknown));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.EqualTo(exception));
        }

        private IObjectValidator CreateSut()
        {
            return new BusinessLogic.Validation.ObjectValidator();
        }
    }
}
