using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorResponseModelResolver
{
    [TestFixture]
    public class ResolveWithIntranetValidationExceptionTests
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
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenIntranetValidationExceptionIsNull_ThrowsArgumentNullException(bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("intranetValidationException"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToResponseType_ReturnsNotNull(bool withState)
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(validatingField: "ResponseType");
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToResponseType_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message, validatingField: "ResponseType");
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, state);

            result.AssertExpectedValues("unsupported_response_type", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToClientId_ReturnsNotNull(bool withState)
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(validatingField: "ClientId");
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToClientId_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message, validatingField: "ClientId");
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, state);

            result.AssertExpectedValues("invalid_request", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToRedirectUri_ReturnsNotNull(bool withState)
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(validatingField: "RedirectUri");
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToRedirectUri_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message, validatingField: "RedirectUri");
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, state);

            result.AssertExpectedValues("invalid_request", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToState_ReturnsNotNull(bool withState)
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(validatingField: "State");
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToState_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message, validatingField: "State");
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, state);

            result.AssertExpectedValues("invalid_request", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToScopes_ReturnsNotNull(bool withState)
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(validatingField: "Scopes");
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsEqualToScopes_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message, validatingField: "Scopes");
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, state);

            result.AssertExpectedValues("invalid_scope", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsUnknown_ReturnsNotNull(bool withState)
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(validatingField: _fixture.Create<string>());
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetValidationExceptionWhereValidationFieldIsUnknown_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message, validatingField: _fixture.Create<string>());
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetValidationException, state);

            result.AssertExpectedValues("invalid_request", message, null, state);
        }

        private IntranetValidationException CreateIntranetValidationException(string message = null, string validatingField = null)
        {
            return new IntranetValidationException(_fixture.Create<ErrorCode>(), message ?? _fixture.Create<string>())
            {
                ValidatingField = validatingField ?? _fixture.Create<string>()
            };
        }
    }
}