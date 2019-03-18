using System;
using System.Reflection;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.TestHelpers;

namespace OSDevGrp.OSIntranet.Core.Tests.IntranetExceptionBuilder
{
    [TestFixture]
    public class BuildTests
    {
        #region Private variables

        private ErrorCodeAttributeTestHelper _errorCodeAttributeTestHelper;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _errorCodeAttributeTestHelper = new ErrorCodeAttributeTestHelper();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetRepositoryException_ReturnsIntranetRepositoryException()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.RepositoryError, _fixture.Create<string>(), _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result, Is.TypeOf<IntranetRepositoryException>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetRepositoryException_AssertErrorCodeIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.RepositoryError;

            IIntranetExceptionBuilder sut = CreateSut(errorCode, _fixture.Create<string>(), _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.ErrorCode, Is.EqualTo(errorCode));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetRepositoryException_AssertMessageIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.RepositoryError;
            string methodName = _fixture.Create<string>();
            string error = _fixture.Create<string>();

            ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);

            IIntranetExceptionBuilder sut = CreateSut(errorCode, methodName, error);

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.Message, Is.EqualTo(string.Format(errorCodeAttribute.Message, methodName, error)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetRepositoryExceptionWithoutInnerException_AssertInnerExceptionIsNull()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.RepositoryError, _fixture.Create<string>(), _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetRepositoryExceptionWithInnerException_AssertInnerExceptionIsCorrect()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.RepositoryError, _fixture.Create<string>(), _fixture.Create<string>());

            Exception innerException = _fixture.Create<Exception>();
            IntranetExceptionBase result = sut.WithInnerException(innerException).Build();

            Assert.That(result.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetRepositoryExceptionWithoutMethodBase_AssertMethodBaseIsNull()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.RepositoryError, _fixture.Create<string>(), _fixture.Create<string>());

            IntranetRepositoryException result = (IntranetRepositoryException) sut.Build();

            Assert.That(result.MethodBase, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetRepositoryExceptionWithMethodBase_AssertMethodBaseIsCorrect()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.RepositoryError, _fixture.Create<string>(), _fixture.Create<string>());

            MethodBase methodBase = MethodBase.GetCurrentMethod();
            IntranetRepositoryException result = (IntranetRepositoryException) sut.WithMethodBase(methodBase).Build();

            Assert.That(result.MethodBase, Is.EqualTo(methodBase));
        }

        private IIntranetExceptionBuilder CreateSut(ErrorCode errorCode, params object[] argumentCollection)
        {
            return new Core.IntranetExceptionBuilder(errorCode, argumentCollection);
        }
    }
}
