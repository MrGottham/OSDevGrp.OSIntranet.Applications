using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
{
	[TestFixture]
    public class BuildTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemException()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToValueNotSetByNamedMethod()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotSetByNamedMethod));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereMessageContainsWithTokenType()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains("'WithTokenType'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereMessageContainsTypeNameForTokenBuilder()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains($"'{sut.GetType().Name}'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemException()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToValueNotSetByNamedMethod()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotSetByNamedMethod));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereMessageContainsWithAccessToken()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains("'WithAccessToken'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereMessageContainsTypeNameForTokenBuilder()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains($"'{sut.GetType().Name}'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenRefreshTokenIsNotSetWithRefreshToken_ThrowsIntranetSystemException()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenRefreshTokenIsNotSetWithRefreshToken_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToValueNotSetByNamedMethod()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotSetByNamedMethod));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenRefreshTokenIsNotSetWithRefreshToken_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenRefreshTokenIsNotSetWithRefreshToken_ThrowsIntranetSystemExceptionWhereMessageContainsWithRefreshToken()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains("'WithRefreshToken'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenRefreshTokenIsNotSetWithRefreshToken_ThrowsIntranetSystemExceptionWhereMessageContainsTypeNameForTokenBuilder()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains($"'{sut.GetType().Name}'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenExpiresIsNotSetWithExpires_ThrowsIntranetSystemException()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenExpiresIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToValueNotSetByNamedMethod()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotSetByNamedMethod));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenExpiresIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenExpiresIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereMessageContainsWithExpires()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains("'WithExpires'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenExpiresIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereMessageContainsTypeNameForTokenBuilder()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>());

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains($"'{sut.GetType().Name}'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereTokenTypeIsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.TokenType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereTokenTypeIsNotEmpty()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.TokenType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereTokenTypeIsEqualToValueFromWithTokenType()
        {
            string tokenType = _fixture.Create<string>();
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(tokenType)
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.TokenType, Is.EqualTo(tokenType));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.AccessToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereAccessTokenIsNotEmpty()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.AccessToken, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereAccessTokenIsEqualToValueFromWithAccessToken()
        {
            string accessToken = _fixture.Create<string>();
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(accessToken)
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.RefreshToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereRefreshTokenIsNotEmpty()
        {
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.RefreshToken, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereRefreshTokenIsEqualToValueFromWithAccessToken()
        {
            string refreshToken = _fixture.Create<string>();
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(refreshToken)
                .WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

            IRefreshableToken result = sut.Build();

            Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAllTokenValuesItSet_ReturnsRefreshableTokenWhereExpiresIsEqualToValueFromWithExpires()
        {
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
            ITokenBuilder<IRefreshableToken> sut = CreateSut()
                .WithTokenType(_fixture.Create<string>())
                .WithAccessToken(_fixture.Create<string>())
                .WithRefreshToken(_fixture.Create<string>())
                .WithExpires(expires);

            IRefreshableToken result = sut.Build();

            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        private static ITokenBuilder<IRefreshableToken> CreateSut()
        {
            return new Domain.Security.RefreshableTokenCreator();
        }
    }
}