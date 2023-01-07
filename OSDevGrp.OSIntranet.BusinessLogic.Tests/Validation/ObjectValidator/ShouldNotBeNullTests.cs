using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.ObjectValidator
{
    [TestFixture]
    public class ShouldNotBeNullTests
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
        public void ShouldNotBeNull_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<object>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<object>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<object>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<object>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValueIsNull_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldNotBeNull<object>(null, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValueIsNotNull_ReturnsObjectValidator()
        {
            IObjectValidator sut = CreateSut();

            IValidator result = sut.ShouldNotBeNull(_fixture.Create<object>(), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.ObjectValidator>());
        }

        private IObjectValidator CreateSut()
        {
            return new BusinessLogic.Validation.ObjectValidator();
        }
    }
}