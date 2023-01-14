using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.DateTimeValidator
{
    [TestFixture]
    public class ShouldBeFutureDateOrTodayTests
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
        public void ShouldBeFutureDateOrToday_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeFutureDateOrToday(_fixture.Create<DateTime>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeFutureDateOrToday_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeFutureDateOrToday(_fixture.Create<DateTime>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeFutureDateOrToday_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeFutureDateOrToday(_fixture.Create<DateTime>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeFutureDateOrToday_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeFutureDateOrToday(_fixture.Create<DateTime>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeFutureDateOrToday_WhenValueDateIsBeforeToday_ThrowsIntranetValidationException()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeFutureDateOrToday(value, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeFutureDateOrToday));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeFutureDateOrToday_WhenValueDateIsToday_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Today;
            IValidator result = sut.ShouldBeFutureDateOrToday(value, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeFutureDateOrToday_WhenValueDateIsAfterToday_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Today.AddDays(_random.Next(1, 7));
            IValidator result = sut.ShouldBeFutureDateOrToday(value, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        private IDateTimeValidator CreateSut()
        {
            return new BusinessLogic.Validation.DateTimeValidator();
        }
    }
}