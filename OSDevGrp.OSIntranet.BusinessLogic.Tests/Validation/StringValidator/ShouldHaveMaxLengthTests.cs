using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.StringValidator
{
    [TestFixture]
    public class ShouldHaveMaxLengthTests
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
        public void ShouldHaveMaxLength_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxLength(_fixture.Create<string>(), _fixture.Create<int>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxLength(_fixture.Create<string>(), _fixture.Create<int>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxLength(_fixture.Create<string>(), _fixture.Create<int>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMaxLength(_fixture.Create<string>(), _fixture.Create<int>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValueIsNullAndNullIsAllowed_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            IValidator result = sut.ShouldHaveMaxLength(null, _fixture.Create<int>(), GetType(), _fixture.Create<string>(), true);

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldHaveMaxLength(null, _fixture.Create<int>(), validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValueLengthIsLowerThanMaxLength_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            string value = _fixture.Create<string>();
            int maxLength = value.Length + _random.Next(1, 10);
            IValidator result = sut.ShouldHaveMaxLength(value, maxLength, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValueLengthIsEqualToMaxLength_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            string value = _fixture.Create<string>();
            int maxLength = value.Length;
            IValidator result = sut.ShouldHaveMaxLength(value, maxLength, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldHaveMaxLength_WhenValueLengthIsGreaterThanMaxLength_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            string value = _fixture.Create<string>();
            int maxLength = value.Length - _random.Next(1, value.Length);
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldHaveMaxLength(value, maxLength, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldHaveMaxLength));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        private IStringValidator CreateSut()
        {
            return new BusinessLogic.Validation.StringValidator();
        }
    }
}