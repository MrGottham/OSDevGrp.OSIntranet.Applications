using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.DateTimeValidator
{
    [TestFixture]
    public class ShouldBeLaterThanOrEqualToOffsetDateTests
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
        public void ShouldBeLaterThanOrEqualToOffsetDate_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOrEqualToOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOrEqualToOffsetDate_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOrEqualToOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOrEqualToOffsetDate_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOrEqualToOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOrEqualToOffsetDate_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOrEqualToOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOrEqualToOffsetDate_WhenValueDateIsLaterThanOffsetDate_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime offsetDate = _random.Next(100) > 50 ? DateTime.Today : DateTime.Today.AddDays(_random.Next(1, 7));
            DateTime value = offsetDate.AddDays(_random.Next(1, 7));
            IValidator result = sut.ShouldBeLaterThanOrEqualToOffsetDate(value, offsetDate, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOrEqualToOffsetDate_WhenValueDateIsEqualToOffsetDate_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime offsetDate = _random.Next(100) > 50 ? DateTime.Today : DateTime.Today.AddDays(_random.Next(1, 7));
            DateTime value = offsetDate;
            IValidator result = sut.ShouldBeLaterThanOrEqualToOffsetDate(value, offsetDate, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOrEqualToOffsetDate_WhenValueDateIsBeforeOffsetDate_ThrowsIntranetValidationException()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime offsetDate = _random.Next(100) > 50 ? DateTime.Today : DateTime.Today.AddDays(_random.Next(1, 7));
            DateTime value = offsetDate.AddDays(_random.Next(1, 7) * -1);
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeLaterThanOrEqualToOffsetDate(value, offsetDate, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeLaterThanOrEqualToOffsetDate));
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