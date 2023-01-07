using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.DateTimeValidator
{
    [TestFixture]
    public class ShouldBePastDateOrTodayTests
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
        public void ShouldBePastDateOrToday_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateOrToday(_fixture.Create<DateTime>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateOrToday_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateOrToday(_fixture.Create<DateTime>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateOrToday_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateOrToday(_fixture.Create<DateTime>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateOrToday_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateOrToday(_fixture.Create<DateTime>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateOrToday_WhenValueDateIsBeforeToday_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            IValidator result = sut.ShouldBePastDateOrToday(value, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateOrToday_WhenValueDateIsToday_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Today;
            IValidator result = sut.ShouldBePastDateOrToday(value, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateOrToday_WhenValueDateIsAfterToday_ThrowsIntranetValidationException()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Today.AddDays(_random.Next(1, 7));
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBePastDateOrToday(value, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBePastDateOrToday));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        private IDateTimeValidator CreateSut()
        {
            return new BusinessLogic.Validation.DateTimeValidator();
        }
    }
}