using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.DateTimeValidator
{
    [TestFixture]
    public class ShouldBePastDateTimeTests
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
        public void ShouldBePastDateTime_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateTime(_fixture.Create<DateTime>(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateTime_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateTime(_fixture.Create<DateTime>(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateTime_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateTime(_fixture.Create<DateTime>(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateTime_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IDateTimeValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBePastDateTime(_fixture.Create<DateTime>(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateTime_WhenValueDateTimeIsBeforeNow_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Now.AddSeconds(_random.Next(5, 30) * -1);
            IValidator result = sut.ShouldBePastDateTime(value, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateTime_WhenValueDateTimeIsNow_ReturnsDateTimeValidator()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Now;
            IValidator result = sut.ShouldBePastDateTime(value, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.DateTimeValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBePastDateTime_WhenValueDateTimeIsAfterNow_ThrowsIntranetValidationException()
        {
            IDateTimeValidator sut = CreateSut();

            DateTime value = DateTime.Now.AddSeconds(_random.Next(5, 30));
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBePastDateTime(value, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBePastDateTime));
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