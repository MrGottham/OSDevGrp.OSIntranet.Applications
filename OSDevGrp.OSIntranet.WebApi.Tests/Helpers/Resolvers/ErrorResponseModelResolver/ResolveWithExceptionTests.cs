using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorResponseModelResolver
{
    [TestFixture]
    public class ResolveWithExceptionTests
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
        public void Resolve_WhenExceptionIsNull_ThrowsArgumentNullException(bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve((Exception) null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("exception"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithException_ReturnsNotNull(bool withState)
        {
            Exception exception = CreateIntranetValidationException();
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(exception, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithException_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            Exception exception = CreateIntranetValidationException(message);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(exception, state);

            result.AssertExpectedValues("server_error", message, null, state);
        }

        private Exception CreateIntranetValidationException(string message = null)
        {
            return new Exception(message ?? _fixture.Create<string>());
        }
    }
}