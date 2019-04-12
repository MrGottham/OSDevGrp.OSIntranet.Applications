using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.Tests.IntranetExceptionBuilder
{
    [TestFixture]
    public class WithValidatingTypeTests
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
        public void WithValidatingType_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithValidatingType(null));

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithValidatingType_WhenValidatingTypeIsNotNull_ReturnsIntranetExceptionBuilder()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            IIntranetExceptionBuilder result = sut.WithValidatingType(typeof(WithValidatingTypeTests));

            Assert.That(result, Is.EqualTo(sut));
        }

        private IIntranetExceptionBuilder CreateSut()
        {
            return new Core.IntranetExceptionBuilder(_fixture.Create<ErrorCode>());
        }
    }
}
