using System;
using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AcquireTokenTests
    {
        #region Private variables

        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IAcmeChallengeResolver> _acmeChallengeResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _claimResolverMock = new Mock<IClaimResolver>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _acmeChallengeResolverMock = new Mock<IAcmeChallengeResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(null));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(null));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(null));

            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToGrantType()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(null));

            Assert.That(result.ValidatingField, Is.EqualTo("grantType"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(string.Empty));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(string.Empty));

            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToGrantType()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(string.Empty));

            Assert.That(result.ValidatingField, Is.EqualTo("grantType"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(" "));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(" "));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(" "));

            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToGrantType()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcquireToken(" "));

            Assert.That(result.ValidatingField, Is.EqualTo("grantType"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNotNullEmptyOrWhiteSpace_AssertGetTokenWasCalledOnClaimResolver()
        {
            Controller sut = CreateSut();

            sut.AcquireToken(GetLegalGrantType());

            _claimResolverMock.Verify(m => m.GetToken<IToken>(It.IsNotNull<Func<string, string>>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNotEqualToClientCredentials_ThrowsIntranetBusinessException()
        {
            Controller sut = CreateSut();

            Assert.Throws<IntranetBusinessException>(() => sut.AcquireToken(_fixture.Create<string>()));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNotEqualToClientCredentials_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveJwtBearerTokenForAuthenticatedUser()
        {
            Controller sut = CreateSut();

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.AcquireToken(_fixture.Create<string>()));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenTokenWasNotResolved_ThrowsIntranetBusinessException()
        {
            Controller sut = CreateSut(false);

            Assert.Throws<IntranetBusinessException>(() => sut.AcquireToken(GetLegalGrantType()));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenTokenWasNotResolved_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveJwtBearerTokenForAuthenticatedUser()
        {
            Controller sut = CreateSut(false);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.AcquireToken(GetLegalGrantType()));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenAuthenticationValuesWasResolved_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<AccessTokenModel> result = sut.AcquireToken(GetLegalGrantType());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenAuthenticationValuesWasResolved_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<AccessTokenModel> result = sut.AcquireToken(GetLegalGrantType());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenAuthenticationValuesWasResolved_ReturnsOkObjectResultWhereValueIsAccessTokenModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) sut.AcquireToken(GetLegalGrantType()).Result;

            Assert.That(result.Value, Is.TypeOf<AccessTokenModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenAuthenticationValuesWasResolved_ReturnsOkObjectResultWithAccessTokenModelWhereTokenTypeIsEqualToTokenTypeFromToken()
        {
            string tokenType = _fixture.Create<string>();
            IToken token = _fixture.BuildTokenMock(tokenType).Object;
            Controller sut = CreateSut(token: token);

            AccessTokenModel result = (AccessTokenModel) ((OkObjectResult) sut.AcquireToken(GetLegalGrantType()).Result).Value;

            Assert.That(result.TokenType, Is.EqualTo(tokenType));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenAuthenticationValuesWasResolved_ReturnsOkObjectResultWithAccessTokenModelWhereAccessTokenIsEqualToAccessTokenFromToken()
        {
            string accessToken = _fixture.Create<string>();
            IToken token = _fixture.BuildTokenMock(accessToken: accessToken).Object;
            Controller sut = CreateSut(token: token);

            AccessTokenModel result = (AccessTokenModel) ((OkObjectResult) sut.AcquireToken(GetLegalGrantType()).Result).Value;

            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenAuthenticationValuesWasResolved_ReturnsOkObjectResultWithAccessTokenModelWhereExpiresIsEqualToExpiresFromToken()
        {
            DateTime expires = DateTime.Today.AddMinutes(5);
            IToken token = _fixture.BuildTokenMock(expires: expires).Object;
            Controller sut = CreateSut(token: token);

            AccessTokenModel result = (AccessTokenModel) ((OkObjectResult) sut.AcquireToken(GetLegalGrantType()).Result).Value;

            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        private Controller CreateSut(bool hasToken = true, IToken token = null)
        {
            _claimResolverMock.Setup(m => m.GetToken<IToken>(It.IsAny<Func<string, string>>()))
                .Returns(hasToken ? token ?? _fixture.BuildTokenMock().Object : null);

            return new Controller(_claimResolverMock.Object, _dataProtectionProviderMock.Object, _acmeChallengeResolverMock.Object);
        }

        private string GetLegalGrantType() => "client_credentials";
    }
}