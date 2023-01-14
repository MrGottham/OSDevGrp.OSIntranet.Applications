using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.StringValidator
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
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<string>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<string>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<string>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotBeNull(_fixture.Create<string>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValueIsNull_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldNotBeNull(null, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValueIsEmpty_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            IValidator result = sut.ShouldNotBeNull(string.Empty, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValueIsWhiteSpace_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            IValidator result = sut.ShouldNotBeNull(" ", GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldNotBeNull_WhenValueIsNotNullEmptyOrWhiteSpace_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            IValidator result = sut.ShouldNotBeNull(_fixture.Create<string>(), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        private IStringValidator CreateSut()
        {
            return new BusinessLogic.Validation.StringValidator();
        }
    }
}