using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.Tests.IntranetExceptionBuilder
{
    [TestFixture]
    public class WithInnerExceptionTests
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
        public void WithInnerException_WhenInnerExceptionIsNull_ThrowsArgumentNullException()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithInnerException(null));

            Assert.That(result.ParamName, Is.EqualTo("innerException"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithInnerException_WhenInnerExceptionIsNotNull_ReturnsIntranetExceptionBuilder()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            IIntranetExceptionBuilder result = sut.WithInnerException(_fixture.Create<Exception>());

            Assert.That(result, Is.EqualTo(sut));
        }

        private IIntranetExceptionBuilder CreateSut()
        {
            return new Core.IntranetExceptionBuilder(_fixture.Create<ErrorCode>());
        }
    }
}
