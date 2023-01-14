using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.StringValidator
{
    [TestFixture]
    public class ShouldMatchPatternTests
    {
        #region Private variables

        private Fixture _fixture;
        private Regex _pattern;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _pattern = new Regex(@"^DK$", RegexOptions.Compiled);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenPatternIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldMatchPattern(_fixture.Create<string>(), null, GetType(), _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("pattern"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldMatchPattern(_fixture.Create<string>(), _pattern, null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldMatchPattern(_fixture.Create<string>(), _pattern, GetType(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldMatchPattern(_fixture.Create<string>(), _pattern, GetType(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IStringValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldMatchPattern(_fixture.Create<string>(), _pattern, GetType(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValueIsNullAndNullIsAllowed_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            IValidator result = sut.ShouldMatchPattern(null, _pattern, GetType(), _fixture.Create<string>(), true);

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
        {
            IStringValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldMatchPattern(null, _pattern, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValueDoesMatchPattern_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            const string value = "DK";
            IValidator result = sut.ShouldMatchPattern(value, _pattern, GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.StringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldMatchPattern_WhenValueDoesNotMatchPattern_ReturnsStringValidator()
        {
            IStringValidator sut = CreateSut();

            string value = _fixture.Create<string>();
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldMatchPattern(value, _pattern, validatingType, validatingField));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldMatchPattern));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            // ReSharper restore PossibleNullReferenceException
        }

        private IStringValidator CreateSut()
        {
            return new BusinessLogic.Validation.StringValidator();
        }
    }
}