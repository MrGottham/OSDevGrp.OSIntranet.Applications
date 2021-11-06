using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.EnumerableValidator
{
    [TestFixture]
    public class ShouldContainItemsTests
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
        public void ShouldContainItems_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IEnumerableValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldContainItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IEnumerableValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldContainItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IEnumerableValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldContainItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IEnumerableValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldContainItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValueIsNullAndNullIsAllowed_ReturnsNotNull()
        {
            IEnumerableValidator sut = CreateSut();

            IValidator result = sut.ShouldContainItems<object>(null, GetType(), _fixture.Create<string>(), true);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValueIsNullAndNullIsAllowed_ReturnsEnumerableValidator()
        {
            IEnumerableValidator sut = CreateSut();

            IValidator result = sut.ShouldContainItems<object>(null, GetType(), _fixture.Create<string>(), true);

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.EnumerableValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
        {
            IEnumerableValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldContainItems<object>(null, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValueIsNotNullAndValueContainsItems_ReturnsNotNull()
        {
            IEnumerableValidator sut = CreateSut();

            IValidator result = sut.ShouldContainItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValueIsNotNullAndValueContainsItems_ReturnsEnumerableValidator()
        {
            IEnumerableValidator sut = CreateSut();

            IValidator result = sut.ShouldContainItems(_fixture.CreateMany<object>(_random.Next(5, 10)).ToArray(), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.EnumerableValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldContainItems_WhenValueIsNotNullAndValueDoesNotContainItems_ThrowsIntranetValidationException()
        {
            IEnumerableValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldContainItems(Array.Empty<object>(), validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldContainSomeItems));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        private IEnumerableValidator CreateSut()
        {
            return new BusinessLogic.Validation.EnumerableValidator();
        }
    }
}