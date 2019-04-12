using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.Tests.IntranetExceptionBuilder
{
    [TestFixture]
    public class WithValidatingFieldTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithValidatingField_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithValidatingField(null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithValidatingField_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithValidatingField(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithValidatingField_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithValidatingField(" "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithValidatingType_WhenValidatingFieldIsNotNullEmptyOrWhiteSpace_ReturnsIntranetExceptionBuilder()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            IIntranetExceptionBuilder result = sut.WithValidatingField(_fixture.Create<string>());

            Assert.That(result, Is.EqualTo(sut));
        }

        private IIntranetExceptionBuilder CreateSut()
        {
            return new Core.IntranetExceptionBuilder(_fixture.Create<ErrorCode>());
        }
    }
}
