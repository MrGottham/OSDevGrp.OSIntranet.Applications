using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class CreatePostalCodeTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();

            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));
        }

        [Test]
        [Category("UnitTest")]
        public void CreatePostalCode_WhenCalledWithCountryCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreatePostalCode((string) null));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreatePostalCode_WhenCalledWithCountryCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreatePostalCode(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreatePostalCode_WhenCalledWithCountryCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreatePostalCode(" "));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithCountryCodeNotEqualToNullEmptyOrWhiteSpace_AssertQueryAsyncWasCalledOnQueryBusWithGetCountryQuery()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            await sut.CreatePostalCode(countryCode);

            _queryBusMock.Verify(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.Is<IGetCountryQuery>(query => query != null && string.CompareOrdinal(query.CountryCode, countryCode.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithUnknownCountryCode_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            IActionResult result = await sut.CreatePostalCode(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithUnknownCountryCode_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreatePostalCode(_fixture.Create<string>());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithUnknownCountryCode_ReturnsRedirectToActionResultWhereActionNameIsEqualToPostalCodes()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreatePostalCode(_fixture.Create<string>());

            Assert.That(result.ActionName, Is.EqualTo("PostalCodes"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithKnownCountryCode_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreatePostalCode(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithKnownCountryCode_ReturnsViewResultWhereViewNameIsEqualToCreatePostalCode()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.CreatePostalCode(_fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("CreatePostalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithKnownCountryCode_ReturnsViewResultWhereModelIsPostalCodeViewModel()
        {
            string countryCode = _fixture.Create<string>();
            ICountry country = _fixture.BuildCountryMock(countryCode).Object;
            Controller sut = CreateSut(country: country);

            ViewResult result = (ViewResult) await sut.CreatePostalCode(countryCode);

            Assert.That(result.Model, Is.TypeOf<PostalCodeViewModel>());

            PostalCodeViewModel postalCodeViewModel = (PostalCodeViewModel) result.Model;

            Assert.That(postalCodeViewModel, Is.Not.Null);
            Assert.That(postalCodeViewModel.Country, Is.Not.Null);
            Assert.That(postalCodeViewModel.Country.Code, Is.EqualTo(countryCode));
            Assert.That(postalCodeViewModel.Code, Is.Null);
            Assert.That(postalCodeViewModel.City, Is.Null);
            Assert.That(postalCodeViewModel.State, Is.Null);
            Assert.That(postalCodeViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public void CreatePostalCode_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreatePostalCode((PostalCodeViewModel) null));

            Assert.That(result.ParamName, Is.EqualTo("postalCodeViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            await sut.CreatePostalCode(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreatePostalCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            IActionResult result = await sut.CreatePostalCode(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreatePostalCode()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreatePostalCode(model);

            Assert.That(result.ViewName, Is.EqualTo("CreatePostalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreatePostalCode(model);

            Assert.That(result.Model, Is.TypeOf<PostalCodeViewModel>());

            PostalCodeViewModel postalCodeViewModel = (PostalCodeViewModel) result.Model;

            Assert.That(postalCodeViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            await sut.CreatePostalCode(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreatePostalCodeCommand>(command =>
                    string.CompareOrdinal(command.CountryCode, model.Country.Code.ToUpper()) == 0 &&
                    string.CompareOrdinal(command.PostalCode, model.Code) == 0 &&
                    string.CompareOrdinal(command.City, model.City) == 0 &&
                    string.CompareOrdinal(command.State, model.State) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            IActionResult result = await sut.CreatePostalCode(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreatePostalCode(model);

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToPostalCodes()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreatePostalCode(model);

            Assert.That(result.ActionName, Is.EqualTo("PostalCodes"));
        }

        private Controller CreateSut(bool knownCountryCode = true, ICountry country = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.IsAny<IGetCountryQuery>()))
                .Returns(Task.Run(() => knownCountryCode ? country ?? _fixture.BuildCountryMock().Object : null));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreatePostalCodeCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _tokenHelperFactoryMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private PostalCodeViewModel CreateModel()
        {
            return _fixture.Create<PostalCodeViewModel>();
        }
    }
}