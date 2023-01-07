using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.StringValidator
{
    [TestFixture]
    public class ShouldNotBeNullOrWhiteSpaceTests
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
        public void ShouldNotBeNullOrWhiteSpace_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNullOrWhiteSpace(_fixture.Create<string>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNullOrWhiteSpace_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNullOrWhiteSpace(_fixture.Create<string>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNullOrWhiteSpace_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNullOrWhiteSpace(_fixture.Create<string>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNullOrWhiteSpace_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNullOrWhiteSpace(_fixture.Create<string>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNullOrWhiteSpace_WhenValueIsNull_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldNotBeNullOrWhiteSpace(null, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNullOrWhiteSpace_WhenValueIsEmpty_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldNotBeNullOrWhiteSpace(string.Empty, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNullOrWhiteSpace_WhenValueIsWhiteSpace_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldNotBeNullOrWhiteSpace(" ", validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNullOrWhiteSpace_WhenValueIsNotNullEmptyOrWhiteSpace_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            IValidator result = sut.ShouldNotBeNullOrWhiteSpace(_fixture.Create<string>(), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        private IStringValidator CreateSut()
        {
            return new BusinessLogic.Validation.StringValidator();
        }
    }
}