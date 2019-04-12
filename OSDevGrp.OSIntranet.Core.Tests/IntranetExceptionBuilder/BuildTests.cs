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

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetCommandBusException_ReturnsIntranetCommandBusException()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType, _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result, Is.TypeOf<IntranetCommandBusException>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetCommandBusException_AssertErrorCodeIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType;

            IIntranetExceptionBuilder sut = CreateSut(errorCode, _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.ErrorCode, Is.EqualTo(errorCode));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetCommandBusException_AssertMessageIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType;
            string commandTypeName = _fixture.Create<string>();

            ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);

            IIntranetExceptionBuilder sut = CreateSut(errorCode, commandTypeName);

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.Message, Is.EqualTo(string.Format(errorCodeAttribute.Message, commandTypeName)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetCommandBusExceptionWithoutInnerException_AssertInnerExceptionIsNull()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType, _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetCommandBusExceptionWithInnerException_AssertInnerExceptionIsCorrect()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType, _fixture.Create<string>());

            Exception innerException = _fixture.Create<Exception>();
            IntranetExceptionBase result = sut.WithInnerException(innerException).Build();

            Assert.That(result.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetQueryBusException_ReturnsIntranetQueryBusException()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.NoQueryHandlerSupportingQuery, _fixture.Create<string>(), _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result, Is.TypeOf<IntranetQueryBusException>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetQueryBusException_AssertErrorCodeIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.NoQueryHandlerSupportingQuery;

            IIntranetExceptionBuilder sut = CreateSut(errorCode, _fixture.Create<string>(), _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.ErrorCode, Is.EqualTo(errorCode));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetQueryBusException_AssertMessageIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.NoQueryHandlerSupportingQuery;
            string queryTypeName = _fixture.Create<string>();
            string resultTypeName = _fixture.Create<string>();

            ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);

            IIntranetExceptionBuilder sut = CreateSut(errorCode, queryTypeName, resultTypeName);

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.Message, Is.EqualTo(string.Format(errorCodeAttribute.Message, queryTypeName, resultTypeName)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetQueryBusExceptionWithoutInnerException_AssertInnerExceptionIsNull()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.NoQueryHandlerSupportingQuery, _fixture.Create<string>(), _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetQueryBusExceptionWithInnerException_AssertInnerExceptionIsCorrect()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.NoQueryHandlerSupportingQuery, _fixture.Create<string>(), _fixture.Create<string>());

            Exception innerException = _fixture.Create<Exception>();
            IntranetExceptionBase result = sut.WithInnerException(innerException).Build();

            Assert.That(result.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationException_ReturnsIntranetValidationException()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.ValueNotGreaterThanZero, _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result, Is.TypeOf<IntranetValidationException>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationException_AssertErrorCodeIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.ValueNotGreaterThanZero;

            IIntranetExceptionBuilder sut = CreateSut(errorCode, _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.ErrorCode, Is.EqualTo(errorCode));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationException_AssertMessageIsCorrect()
        {
            const ErrorCode errorCode = ErrorCode.ValueNotGreaterThanZero;
            string validatingField = _fixture.Create<string>();

            ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);

            IIntranetExceptionBuilder sut = CreateSut(errorCode, validatingField);

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.Message, Is.EqualTo(string.Format(errorCodeAttribute.Message, validatingField)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationExceptionWithoutInnerException_AssertInnerExceptionIsNull()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.ValueNotGreaterThanZero, _fixture.Create<string>());

            IntranetExceptionBase result = sut.Build();

            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationExceptionWithInnerException_AssertInnerExceptionIsCorrect()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.ValueNotGreaterThanZero, _fixture.Create<string>());

            Exception innerException = _fixture.Create<Exception>();
            IntranetExceptionBase result = sut.WithInnerException(innerException).Build();

            Assert.That(result.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationExceptionWithoutValidatingType_AssertValidatingTypeIsNull()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.ValueNotGreaterThanZero, _fixture.Create<string>());

            IntranetValidationException result = (IntranetValidationException) sut.Build();

            Assert.That(result.ValidatingType, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationExceptionWithValidatingType_AssertValidatingTypeIsCorrect()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.ValueNotGreaterThanZero, _fixture.Create<string>());

            Type validatingType = GetType();
            IntranetValidationException result = (IntranetValidationException) sut.WithValidatingType(validatingType).Build();

            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationExceptionWithoutValidatingField_AssertValidatingFieldIsNull()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.ValueNotGreaterThanZero, _fixture.Create<string>());

            IntranetValidationException result = (IntranetValidationException) sut.Build();

            Assert.That(result.ValidatingField, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalledForIntranetValidationExceptionWithValidatingField_AssertValidatingFieldIsCorrect()
        {
            IIntranetExceptionBuilder sut = CreateSut(ErrorCode.ValueNotGreaterThanZero, _fixture.Create<string>());

            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = (IntranetValidationException) sut.WithValidatingField(validatingField).Build();

            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        private IIntranetExceptionBuilder CreateSut(ErrorCode errorCode, params object[] argumentCollection)
        {
            return new Core.IntranetExceptionBuilder(errorCode, argumentCollection);
        }
    }
}
