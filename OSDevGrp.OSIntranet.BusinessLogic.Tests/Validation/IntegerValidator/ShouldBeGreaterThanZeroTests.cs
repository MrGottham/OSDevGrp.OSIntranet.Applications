using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.IntegerValidator
{
    [TestFixture]
    public class ShouldBeGreaterThanZeroTests
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
        public void ShouldBeGreaterThanZero_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanZero(_fixture.Create<int>(), null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanZero_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanZero(_fixture.Create<int>(), GetType(), null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanZero_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanZero(_fixture.Create<int>(), GetType(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanZero_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanZero(_fixture.Create<int>(), GetType(), " "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanZero_WhenValueIsGreaterThanZero_ReturnsIntegerValidator()
        {
            IIntegerValidator sut = CreateSut();

            IValidator result = sut.ShouldBeGreaterThanZero(_random.Next(1, 5), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.IntegerValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanZero_WhenValueIsEqualToZero_ThrowsIntranetValidationException()
        {
            IIntegerValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeGreaterThanZero(0, validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotGreaterThanZero));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanZero_WhenValueIsLowerThanZero_ThrowsIntranetValidationException()
        {
            IIntegerValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeGreaterThanZero(_random.Next(1, 5) * -1, validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotGreaterThanZero));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        private IIntegerValidator CreateSut()
        {
            return new BusinessLogic.Validation.IntegerValidator();
        }
    }
}
