using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class DeletePostalCodeTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));
            _fixture.Customize<IPostalCode>(builder => builder.FromFactory(() => _fixture.BuildPostalCodeMock().Object));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCode_WhenCalledWithCountryCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCode(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCode_WhenCalledWithCountryCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCode(string.Empty, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCode_WhenCalledWithCountryCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCode(" ", _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCode_WhenCalledWithPostalCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCode(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCode_WhenCalledWithPostalCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCode(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCode_WhenCalledWithPostalCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCode(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeletePostalCode_WhenCalledWithCountryCodeAndPostalCode_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            string postalCode = _fixture.Create<string>();
            await sut.DeletePostalCode(countryCode, postalCode);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeletePostalCodeCommand>(value => string.CompareOrdinal(value.CountryCode, countryCode.ToUpper()) == 0 && string.CompareOrdinal(value.PostalCode, postalCode) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeletePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeletePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeletePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeletePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeletePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToPostalCodes()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeletePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.ActionName, Is.EqualTo("PostalCodes"));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeletePostalCodeCommand>()))
                .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
