using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.IntegerValidator
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
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_fixture.Create<int>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValueIsGreaterThanZero_ReturnsIntegerValidator()
        {
            IIntegerValidator sut = CreateSut();

            IValidator result = sut.ShouldBeGreaterThanOrEqualToZero(_random.Next(1, 5), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.IntegerValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValueIsEqualToZero_ReturnsIntegerValidator()
        {
            IIntegerValidator sut = CreateSut();

            IValidator result = sut.ShouldBeGreaterThanOrEqualToZero(0, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.IntegerValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeGreaterThanOrEqualToZero_WhenValueIsLowerThanZero_ThrowsIntranetValidationException()
        {
            IIntegerValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeGreaterThanOrEqualToZero(_random.Next(1, 5) * -1, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotGreaterThanOrEqualToZero));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        private IIntegerValidator CreateSut()
        {
            return new BusinessLogic.Validation.IntegerValidator();
        }
    }
}