using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.TestHelpers;

namespace OSDevGrp.OSIntranet.Core.Tests.Enums.ErrorCode
{
    [TestFixture]
    public class ErrorCodeTests
    {
        #region Private variables

        private ErrorCodeAttributeTestHelper _errorCodeAttributeTestHelper;
        private Fixture _fixture;
        private readonly Regex _argumentCounterRegex = new Regex(@"(\{[0-9]\})", RegexOptions.Compiled);

        #endregion

        [SetUp]
        public void SetUp()
        {
            _errorCodeAttributeTestHelper = new ErrorCodeAttributeTestHelper();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ErrorCode_ForAllErrorCodes_EnsureErrorCodeAttributeIsPresent()
        {
            IEnumerable<Interfaces.Enums.ErrorCode> sutCollection = CreateSut();

            IList<Interfaces.Enums.ErrorCode> result = sutCollection.AsParallel()
                .Where(errorCode =>
                {
                    try
                    {
                        return _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode) == null;
                    }
                    catch (ArgumentNullException)
                    {
                        return false;
                    }
                    catch (ArgumentException)
                    {
                        return false;
                    }
                })
                .OrderBy(errorCode => (int) errorCode)
                .ToList();

            if (result.Count == 0)
            {
                return;
            }

            StringBuilder resultBuilder = new StringBuilder();
            foreach (Interfaces.Enums.ErrorCode errorCode in result)
            {
                resultBuilder.AppendLine($"The error code named '{errorCode}' has no '{typeof(ErrorCodeAttribute).Name}'.");
            }

            Assert.Fail(resultBuilder.ToString());
        }

        [Test]
        [Category("UnitTest")]
        public void ErrorCode_ForAllErrorCodes_EnsureMessageIsNotNullEmptyOrWhiteSpace()
        {
            IEnumerable<Interfaces.Enums.ErrorCode> sutCollection = CreateSut();

            IList<Interfaces.Enums.ErrorCode> result = sutCollection.AsParallel()
                .Where(errorCode =>
                {
                    try
                    {
                        ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);
                        return errorCodeAttribute != null && string.IsNullOrWhiteSpace(errorCodeAttribute.Message);
                    }
                    catch (ArgumentNullException ex)
                    {
                        return string.Compare("message", ex.ParamName, StringComparison.Ordinal) == 0;
                    }
                    catch (ArgumentException)
                    {
                        return false;
                    }
                })
                .OrderBy(errorCode => (int) errorCode)
                .ToList();

            if (result.Count == 0)
            {
                return;
            }

            StringBuilder resultBuilder = new StringBuilder();
            foreach (Interfaces.Enums.ErrorCode errorCode in result)
            {
                resultBuilder.AppendLine($"The error code named '{errorCode}' has no valid message.");
            }

            Assert.Fail(resultBuilder.ToString());
        }

        [Test]
        [Category("UnitTest")]
        public void ErrorCode_ForAllErrorCodes_EnsureExceptionTypeIsNotNull()
        {
            IEnumerable<Interfaces.Enums.ErrorCode> sutCollection = CreateSut();

            IList<Interfaces.Enums.ErrorCode> result = sutCollection.AsParallel()
                .Where(errorCode =>
                {
                    try
                    {
                        ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);
                        return errorCodeAttribute != null && errorCodeAttribute.ExceptionType == null;
                    }
                    catch (ArgumentNullException ex)
                    {
                        return string.Compare("exceptionType", ex.ParamName, StringComparison.Ordinal) == 0;
                    }
                    catch (ArgumentException)
                    {
                        return false;
                    }
                })
                .OrderBy(errorCode => (int) errorCode)
                .ToList();

            if (result.Count == 0)
            {
                return;
            }

            StringBuilder resultBuilder = new StringBuilder();
            foreach (Interfaces.Enums.ErrorCode errorCode in result)
            {
                resultBuilder.AppendLine($"The error code named '{errorCode}' has no exception type.");
            }

            Assert.Fail(resultBuilder.ToString());
        }

        [Test]
        [Category("UnitTest")]
        public void ErrorCode_ForAllErrorCodes_EnsureExceptionTypeIsIntranetExceptionBase()
        {
            IEnumerable<Interfaces.Enums.ErrorCode> sutCollection = CreateSut();

            IList<Interfaces.Enums.ErrorCode> result = sutCollection.AsParallel()
                .Where(errorCode =>
                {
                    try
                    {
                        ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);
                        return errorCodeAttribute != null && errorCodeAttribute.ExceptionType.IsSubclassOf(typeof(IntranetExceptionBase)) == false;
                    }
                    catch (ArgumentNullException)
                    {
                        return false;
                    }
                    catch (ArgumentException ex)
                    {
                        return string.Compare("exceptionType", ex.ParamName, StringComparison.Ordinal) == 0;
                    }
                })
                .OrderBy(errorCode => (int) errorCode)
                .ToList();


            if (result.Count == 0)
            {
                return;
            }

            StringBuilder resultBuilder = new StringBuilder();
            foreach (Interfaces.Enums.ErrorCode errorCode in result)
            {
                resultBuilder.AppendLine($"The error code named '{errorCode}' has an invalid exception type which are not based on '{typeof(IntranetExceptionBase).Name}'.");
            }

            Assert.Fail(resultBuilder.ToString());
        }

        [Test]
        [Category("UnitTest")]
        public void ErrorCode_ForAllErrorCodes_EnsureIntranetExceptionBaseCanBeCreated()
        {
            IEnumerable<Interfaces.Enums.ErrorCode> sutCollection = CreateSut();

            IList<Interfaces.Enums.ErrorCode> result = sutCollection.AsParallel()
                .Where(errorCode =>
                {
                    try
                    {
                        ErrorCodeAttribute errorCodeAttribute = _errorCodeAttributeTestHelper.GetErrorCodeAttribute(errorCode);
                        int numberOfArguments = _argumentCounterRegex.Matches(errorCodeAttribute.Message).Count;

                        new Core.IntranetExceptionBuilder(errorCode, _fixture.CreateMany<object>(numberOfArguments).ToArray()).Build();

                        return false;
                    }
                    catch (ArgumentNullException)
                    {
                        return false;
                    }
                    catch (ArgumentException)
                    {
                        return false;
                    }
                })
                .OrderBy(errorCode => (int) errorCode)
                .ToList();

            if (result.Count == 0)
            {
                return;
            }

            StringBuilder resultBuilder = new StringBuilder();
            foreach (Interfaces.Enums.ErrorCode errorCode in result)
            {
                resultBuilder.AppendLine($"Unable to build an '{typeof(IntranetExceptionBase).Name}' for the error code named '{errorCode}'.");
            }

            Assert.Fail(resultBuilder.ToString());
        }

        [Test]
        [Category("UnitTest")]
        public void ErrorCode_ForAllErrorCodes_EnsureAllErrorCodesAreUnique()
        {
            IEnumerable<Interfaces.Enums.ErrorCode> sutCollection = CreateSut();

            int result = sutCollection.AsParallel()
                .Select(errorCode => (int) errorCode)
                .Distinct()
                .Count();

            Assert.That(result, Is.EqualTo(sutCollection.Count()));
        }

        private IEnumerable<Interfaces.Enums.ErrorCode> CreateSut()
        {
            return Enum.GetValues(typeof(Interfaces.Enums.ErrorCode))
                .Cast<Interfaces.Enums.ErrorCode>()
                .OrderBy(errorCode => (int) errorCode)
                .ToList();
        }
    }
}
