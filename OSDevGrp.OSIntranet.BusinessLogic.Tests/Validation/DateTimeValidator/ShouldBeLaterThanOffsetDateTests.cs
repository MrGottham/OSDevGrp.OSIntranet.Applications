using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.DateTimeValidator
{
    [TestFixture]
    public class ShouldBeLaterThanOffsetDateTests
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
        public void ShouldBeLaterThanOffsetDate_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOffsetDate_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), GetType(), null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOffsetDate_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), GetType(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOffsetDate_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeLaterThanOffsetDate(_fixture.Create<DateTime>(), _fixture.Create<DateTime>(), GetType(), " "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOffsetDate_WhenValueDateIsLaterThanOffsetDate_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime offsetDate = _random.Next(100) > 50 ? DateTime.Today : DateTime.Today.AddDays(_random.Next(1, 7));
            DateTime value = offsetDate.AddDays(_random.Next(1, 7));
            IValidator result = sut.ShouldBeLaterThanOffsetDate(value, offsetDate, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOffsetDate_WhenValueDateIsEqualToOffsetDate_ThrowsIntranetValidationException()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime offsetDate = _random.Next(100) > 50 ? DateTime.Today : DateTime.Today.AddDays(_random.Next(1, 7));
            DateTime value = offsetDate;
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeLaterThanOffsetDate(value, offsetDate, validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeLaterThanOffsetDate));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeLaterThanOffsetDate_WhenValueDateIsBeforeOffsetDate_ThrowsIntranetValidationException()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime offsetDate = _random.Next(100) > 50 ? DateTime.Today : DateTime.Today.AddDays(_random.Next(1, 7));
            DateTime value = offsetDate.AddDays(_random.Next(1, 7) * -1);
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeLaterThanOffsetDate(value, offsetDate, validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeLaterThanOffsetDate));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        private IDateTimeValidator CreateSut()
        {
            return new BusinessLogic.Validation.DateTimeValidator();
        }
    }
}
