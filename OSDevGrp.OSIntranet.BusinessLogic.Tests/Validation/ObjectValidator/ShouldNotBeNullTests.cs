using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

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

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<object>(), GetType(), null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<object>(), GetType(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<object>(), GetType(), " "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValueIsNull_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldNotBeNull<object>(null, validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
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
