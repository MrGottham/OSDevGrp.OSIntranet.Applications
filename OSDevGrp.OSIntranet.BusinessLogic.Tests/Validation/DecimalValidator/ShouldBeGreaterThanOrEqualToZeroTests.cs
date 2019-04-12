using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.DecimalValidator
{
    [TestFixture]
    public class ShouldBeGreaterThanOrEqualToZeroTests
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
        public void ShouldBeGreaterThanOrEqualToZero_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IDecimalValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IDecimalValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), GetType(), null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IDecimalValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), GetType(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IDecimalValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), GetType(), " "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValueIsGreaterThanZero_ReturnsDecimalValidator()
        {
            IDecimalValidator sut = CreateSut();

            IValidator result = sut.ShouldBeGreaterThanOrEqualToZero(_random.Next(1, 5), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DecimalValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValueIsEqualToZero_ReturnsDecimalValidator()
        {
            IDecimalValidator sut = CreateSut();

            IValidator result = sut.ShouldBeGreaterThanOrEqualToZero(0M, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DecimalValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValueIsLowerThanZero_ThrowsIntranetValidationException()
        {
            IDecimalValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_random.Next(1, 5) * -1, validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotGreaterThanOrEqualToZero));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        private IDecimalValidator CreateSut()
        {
            return new BusinessLogic.Validation.DecimalValidator();
        }
    }
}
