using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Queries.ExportContactCollectionQuery
{
    [TestFixture]
    public class ToGetContactCollectionQueryTests
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
        public void ToGetContactCollectionQuery_WhenCalled_ReturnsNotNull()
        {
            IExportContactCollectionQuery sut = CreateSut();

            IGetContactCollectionQuery result = sut.ToGetContactCollectionQuery();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToGetContactCollectionQuery_WhenCalled_ReturnsGetContactCollectionQuery()
        {
            IExportContactCollectionQuery sut = CreateSut();

            IGetContactCollectionQuery result = sut.ToGetContactCollectionQuery();

            Assert.That(result, Is.TypeOf<GetContactCollectionQuery>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToGetContactCollectionQuery_WhenCalled_ReturnsGetContactCollectionQueryWithTokenTypeFromExportContactCollectionQuery()
        {
            string tokenType = _fixture.Create<string>();
            IExportContactCollectionQuery sut = CreateSut(tokenType);

            IGetContactCollectionQuery result = sut.ToGetContactCollectionQuery();

            Assert.That(result.TokenType, Is.EqualTo(tokenType));
        }

        [Test]
        [Category("UnitTest")]
        public void ToGetContactCollectionQuery_WhenCalled_ReturnsGetContactCollectionQueryWithAccessTokenFromExportContactCollectionQuery()
        {
            string accessToken = _fixture.Create<string>();
            IExportContactCollectionQuery sut = CreateSut(accessToken: accessToken);

            IGetContactCollectionQuery result = sut.ToGetContactCollectionQuery();

            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        [Category("UnitTest")]
        public void ToGetContactCollectionQuery_WhenCalled_ReturnsGetContactCollectionQueryWithRefreshTokenFromExportContactCollectionQuery()
        {
            string refreshToken = _fixture.Create<string>();
            IExportContactCollectionQuery sut = CreateSut(refreshToken: refreshToken);

            IGetContactCollectionQuery result = sut.ToGetContactCollectionQuery();

            Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
        }

        [Test]
        [Category("UnitTest")]
        public void ToGetContactCollectionQuery_WhenCalled_ReturnsGetContactCollectionQueryWithExpiresFromExportContactCollectionQuery()
        {
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 100));
            IExportContactCollectionQuery sut = CreateSut(expires: expires);

            IGetContactCollectionQuery result = sut.ToGetContactCollectionQuery();

            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        private IExportContactCollectionQuery CreateSut(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return new BusinessLogic.Contacts.Queries.ExportContactCollectionQuery
            {
                TokenType = tokenType ?? _fixture.Create<string>(),
                AccessToken = accessToken ?? _fixture.Create<string>(),
                RefreshToken = refreshToken ?? _fixture.Create<string>(),
                Expires = expires ?? DateTime.Now.AddSeconds(_random.Next(60, 300))
            };
        }
    }
}