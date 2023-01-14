using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.ObjectValidator
{
    [TestFixture]
    public class ShouldBeKnownValueTests
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
        public void ShouldBeKnownValue_WhenIsKnownValueGetterIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeKnownValue(_fixture.Create<object>(), null, GetType(), _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("isKnownValueGetter"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            // ReSharper disable UnusedParameter.Local
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeKnownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), null, _fixture.Create<string>()));
            // ReSharper restore UnusedParameter.Local

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            // ReSharper disable UnusedParameter.Local
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeKnownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), null));
            // ReSharper restore UnusedParameter.Local

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            // ReSharper disable UnusedParameter.Local
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeKnownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), string.Empty));
            // ReSharper restore UnusedParameter.Local

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            // ReSharper disable UnusedParameter.Local
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeKnownValue(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), " "));
            // ReSharper restore UnusedParameter.Local

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValueIsNullAndNullIsAllowed_ReturnsObjectValidator()
        {
            IObjectValidator sut = CreateSut();

            // ReSharper disable UnusedParameter.Local
            IValidator result = sut.ShouldBeKnownValue<object>(null, async obj => await Task.Run(() => _fixture.Create<bool>()), GetType(), _fixture.Create<string>(), true);
            // ReSharper restore UnusedParameter.Local

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.ObjectValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            // ReSharper disable UnusedParameter.Local
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeKnownValue<object>(null, async obj => await Task.Run(() => _fixture.Create<bool>()), validatingType, validatingField));
            // ReSharper restore UnusedParameter.Local

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValueIsNotNull_AssertIsKnownValueGetterWasCalled()
        {
            IObjectValidator sut = CreateSut();

            object value = _fixture.Create<object>();
            object isKnownValueGetterWasCalledWithObject = null;
            sut.ShouldBeKnownValue(value, async obj => await Task.Run(() =>
                {
                    isKnownValueGetterWasCalledWithObject = obj;
                    return true;
                }),
                GetType(),
                _fixture.Create<string>());

            Assert.That(isKnownValueGetterWasCalledWithObject, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValueIsNotNullAndKnown_ReturnsReturnsObjectValidator()
        {
            IObjectValidator sut = CreateSut();

            // ReSharper disable UnusedParameter.Local
            IValidator result = sut.ShouldBeKnownValue(_fixture.Create<object>(), async obj => await Task.Run(() => true), GetType(), _fixture.Create<string>());
            // ReSharper restore UnusedParameter.Local

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.ObjectValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValueIsNotNullAndUnknown_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            // ReSharper disable UnusedParameter.Local
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeKnownValue(_fixture.Create<object>(), async obj => await Task.Run(() => false), validatingType, validatingField));
            // ReSharper restore UnusedParameter.Local

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeKnown));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeKnownValue_WhenValueIsNotNullAndIsKnownValueGetterThrowsException_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Exception exception = _fixture.Create<Exception>();
            bool IsKnownValueGetter() => throw exception;

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            // ReSharper disable UnusedParameter.Local
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeKnownValue(_fixture.Create<object>(), async obj => await Task.Run(IsKnownValueGetter), validatingType, validatingField));
            // ReSharper restore UnusedParameter.Local

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeKnown));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.EqualTo(exception));
            // ReSharper restore PossibleNullReferenceException
        }

        private IObjectValidator CreateSut()
        {
            return new BusinessLogic.Validation.ObjectValidator();
        }
    }
}