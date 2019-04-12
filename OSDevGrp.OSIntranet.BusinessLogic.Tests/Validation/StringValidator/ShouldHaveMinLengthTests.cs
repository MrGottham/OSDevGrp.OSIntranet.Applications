using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.StringValidator
{
    [TestFixture]
    public class ShouldHaveMinLengthTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMinLength(_fixture.Create<string>(), _fixture.Create<int>(), null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMinLength(_fixture.Create<string>(), _fixture.Create<int>(), GetType(), null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMinLength(_fixture.Create<string>(), _fixture.Create<int>(), GetType(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMinLength(_fixture.Create<string>(), _fixture.Create<int>(), GetType(), " "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValueIsNullAndNullIsAllowed_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            IValidator result = sut.ShouldHaveMinLength(null, _fixture.Create<int>(), GetType(), _fixture.Create<string>(), true);

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldHaveMinLength(null, _fixture.Create<int>(), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValueLengthIsGreaterThanMinLength_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            string value = _fixture.Create<string>();
            int minLength = value.Length - _random.Next(1, value.Length);
            IValidator result = sut.ShouldHaveMinLength(value, minLength, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValueLengthIsEqualToMinLength_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            string value = _fixture.Create<string>();
            int minLength = value.Length;
            IValidator result = sut.ShouldHaveMinLength(value, minLength, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMinLength_WhenValueLengthIsLowerThanMinLength_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            string value = _fixture.Create<string>();
            int minLength = value.Length + _random.Next(1, 10);
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldHaveMinLength(value, minLength, validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldHaveMinLength));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        private IStringValidator CreateSut()
        {
            return new BusinessLogic.Validation.StringValidator();
        }
    }
}
