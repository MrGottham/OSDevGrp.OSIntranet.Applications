using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorDescriptionResolver
{
    [TestFixture]
    public class ResolveTests
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
        public void Resolve_WhenArgumentCollectionIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(_fixture.Create<ErrorCode>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("argumentCollection"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ErrorCode.CannotRetrieveAcmeChallengeForToken, 0)]
        [TestCase(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType, 1)]
        [TestCase(ErrorCode.NoCommandHandlerSupportingCommandWithResultType, 2)]
        [TestCase(ErrorCode.ErrorWhilePublishingCommandWithResultType, 3)]
        public void Resolve_WhenCalled_ReturnsNotNull(ErrorCode errorCode, int arguments)
        {
            object[] argumentCollection = arguments > 0 ? _fixture.CreateMany<object>(arguments).ToArray() : [];
            string result = WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(errorCode, argumentCollection);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ErrorCode.CannotRetrieveAcmeChallengeForToken, 0)]
        [TestCase(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType, 1)]
        [TestCase(ErrorCode.NoCommandHandlerSupportingCommandWithResultType, 2)]
        [TestCase(ErrorCode.ErrorWhilePublishingCommandWithResultType, 3)]
        public void Resolve_WhenCalled_ReturnsNotEmpty(ErrorCode errorCode, int arguments)
        {
            object[] argumentCollection = arguments > 0 ? _fixture.CreateMany<object>(arguments).ToArray() : [];
            string result = WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(errorCode, argumentCollection);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ErrorCode.CannotRetrieveAcmeChallengeForToken, 0)]
        [TestCase(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType, 1)]
        [TestCase(ErrorCode.NoCommandHandlerSupportingCommandWithResultType, 2)]
        [TestCase(ErrorCode.ErrorWhilePublishingCommandWithResultType, 3)]
        public void Resolve_WhenCalled_ReturnsErrorDescriptionForErrorCode(ErrorCode errorCode, int arguments)
        {
            object[] argumentCollection = arguments > 0 ? _fixture.CreateMany<object>(arguments).ToArray() : [];
            string result = WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(errorCode, argumentCollection);

            Assert.That(result, Is.EqualTo(GetErrorDescription(errorCode, argumentCollection)));
        }

        private static string GetErrorDescription(ErrorCode errorCode, params object[] argumentCollection)
        {
            NullGuard.NotNull(argumentCollection, nameof(argumentCollection));

            return new IntranetExceptionBuilder(errorCode, argumentCollection).Build().Message;
        }

    }
}