using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.DeleteContactCommand
{
    [TestFixture]
    public class GetExistingContactAsyncTests
    {
        #region Private variables

        private Mock<IMicrosoftGraphRepository> _microsoftGraphRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _microsoftGraphRepositoryMock = new Mock<IMicrosoftGraphRepository>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void GetExistingContactAsync_WhenMicrosoftGraphRepositoryIsNull_ThrowsArgumentNullException()
        {
            IDeleteContactCommand sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetExistingContactAsync(null, _contactRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("microsoftGraphRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetExistingContactAsync_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            IDeleteContactCommand sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetExistingContactAsync(_microsoftGraphRepositoryMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetExistingContactAsync_WhenCalled_AssertGetContactAsyncWasCalledOnMicrosoftGraphRepository()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>();
            string refreshToken = _fixture.Create<string>();
            string externalIdentifier = _fixture.Create<string>();
            IDeleteContactCommand sut = CreateSut(tokenType, accessToken, expires, refreshToken, externalIdentifier);

            await sut.GetExistingContactAsync(_microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);

            _microsoftGraphRepositoryMock.Verify(m => m.GetContactAsync(
                    It.Is<IRefreshableToken>(value => value != null && string.CompareOrdinal(value.TokenType, tokenType) == 0 && string.CompareOrdinal(value.AccessToken, accessToken) == 0 && value.Expires == expires && string.CompareOrdinal(value.RefreshToken, refreshToken) == 0),
                    It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetExistingContactAsync_WhenCalledAndNoContactWasReturnedFromMicrosoftGraphRepository_AssertApplyContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            IDeleteContactCommand sut = CreateSut(hasMicrosoftGraphContact: false);

            await sut.GetExistingContactAsync(_microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);

            _contactRepositoryMock.Verify(m => m.ApplyContactSupplementAsync(It.IsAny<IContact>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetExistingContactAsync_WhenCalledAndNoContactWasReturnedFromMicrosoftGraphRepository_ReturnsNull()
        {
            IDeleteContactCommand sut = CreateSut(hasMicrosoftGraphContact: false);

            IContact result = await sut.GetExistingContactAsync(_microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetExistingContactAsync_WhenCalledAndContactWasReturnedFromMicrosoftGraphRepository_AssertApplyContactSupplementAsyncWasCalledOnContactRepositoryWithContactFromMicrosoftGraphRepository()
        {
            IContact microsoftGraphContact = _fixture.BuildContactMock().Object;
            IDeleteContactCommand sut = CreateSut(microsoftGraphContact: microsoftGraphContact);

            await sut.GetExistingContactAsync(_microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);

            _contactRepositoryMock.Verify(m => m.ApplyContactSupplementAsync(It.Is<IContact>(value => value == microsoftGraphContact)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetExistingContactAsync_WhenCalledAndContactWasReturnedFromMicrosoftGraphRepository_ReturnAppliedContactSupplementFromContactRepository()
        {
            IContact appliedContactSupplement = _fixture.BuildContactMock().Object;
            IDeleteContactCommand sut = CreateSut(appliedContactSupplement: appliedContactSupplement);

            IContact result = await sut.GetExistingContactAsync(_microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(appliedContactSupplement));
        }

        private IDeleteContactCommand CreateSut(string tokenType = null, string accessToken = null, DateTime? expires = null, string refreshToken = null, string externalIdentifier = null, bool hasMicrosoftGraphContact = true, IContact microsoftGraphContact = null, IContact appliedContactSupplement = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.GetContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<string>()))
                .Returns(() => Task.Run(() => hasMicrosoftGraphContact ? microsoftGraphContact ?? _fixture.BuildContactMock().Object : null));
            _contactRepositoryMock.Setup(m => m.ApplyContactSupplementAsync(It.IsAny<IContact>()))
                .Returns(Task.Run(() => appliedContactSupplement ?? _fixture.BuildContactMock().Object));

            return _fixture.Build<BusinessLogic.Contacts.Commands.DeleteContactCommand>()
                .With(m => m.TokenType, tokenType ?? _fixture.Create<string>())
                .With(m => m.AccessToken, accessToken ?? _fixture.Create<string>())
                .With(m => m.Expires, expires ?? _fixture.Create<DateTime>())
                .With(m => m.RefreshToken, refreshToken ?? _fixture.Create<string>())
                .With(m => m.ExternalIdentifier, externalIdentifier ?? _fixture.Create<string>())
                .Create();
        }
    }
}