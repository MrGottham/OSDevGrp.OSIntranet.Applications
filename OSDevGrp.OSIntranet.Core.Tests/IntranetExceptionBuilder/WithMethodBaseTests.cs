using System;
using System.Reflection;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.Tests.IntranetExceptionBuilder
{
    [TestFixture]
    public class WithMethodBaseTests
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
        public void WithMethodBase_WhenMethodBaseIsNull_ThrowsArgumentNullException()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithMethodBase(null));

            Assert.That(result.ParamName, Is.EqualTo("methodBase"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithMethodBase_WhenMethodBaseIsNotNull_ReturnsIntranetExceptionBuilder()
        {
            IIntranetExceptionBuilder sut = CreateSut();

            IIntranetExceptionBuilder result = sut.WithMethodBase(MethodBase.GetCurrentMethod());

            Assert.That(result, Is.EqualTo(sut));
        }

        private IIntranetExceptionBuilder CreateSut()
        {
            return new Core.IntranetExceptionBuilder(_fixture.Create<ErrorCode>());
        }
    }
}
