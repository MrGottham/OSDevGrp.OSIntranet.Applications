﻿using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.IntegerValidator
{
    [TestFixture]
    public class ShouldBeBetweenTests
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
        public void ShouldBeBetween_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = _random.Next(minValue, maxValue);
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeBetween(value, minValue, maxValue, null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = _random.Next(minValue, maxValue);
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeBetween(value, minValue, maxValue, GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = _random.Next(minValue, maxValue);
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeBetween(value, minValue, maxValue, GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = _random.Next(minValue, maxValue);
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeBetween(value, minValue, maxValue, GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValueIsEqualToMinValue_ReturnsIntegerValidator()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = minValue;
            IValidator result = sut.ShouldBeBetween(value, minValue, maxValue, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.IntegerValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValueIsBetweenMinAndMaxValue_ReturnsIntegerValidator()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = _random.Next(minValue, maxValue);
            IValidator result = sut.ShouldBeBetween(value, minValue, maxValue, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.IntegerValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValueIsEqualToMaxValue_ReturnsIntegerValidator()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = maxValue;
            IValidator result = sut.ShouldBeBetween(value, minValue, maxValue, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.IntegerValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValueIsLowerThanMinValue_ThrowsIntranetValidationException()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = minValue - _random.Next(1, 10);
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeBetween(value, minValue, maxValue, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotBetween));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeBetween_WhenValueIsGreaterThanMaxValue_ThrowsIntranetValidationException()
        {
            IIntegerValidator sut = CreateSut();

            int minValue = _random.Next(1, 100);
            int maxValue = minValue + _random.Next(1, 100);
            int value = maxValue + _random.Next(1, 10);
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeBetween(value, minValue, maxValue, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotBetween));
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