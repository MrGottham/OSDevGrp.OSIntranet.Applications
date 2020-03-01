using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class CreateCountryTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void CreateCountry_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreateCountry();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateCountry_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreateCountry()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateCountry();

            Assert.That(result.ViewName, Is.EqualTo("CreateCountry"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateCountry_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsCountryViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateCountry();

            Assert.That(result.Model, Is.TypeOf<CountryViewModel>());

            CountryViewModel countryViewModel = (CountryViewModel) result.Model;

            Assert.That(countryViewModel, Is.Not.Null);
            Assert.That(countryViewModel.Code, Is.Null);
            Assert.That(countryViewModel.Name, Is.Null);
            Assert.That(countryViewModel.UniversalName, Is.Null);
            Assert.That(countryViewModel.PhonePrefix, Is.Null);
            Assert.That(countryViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateCountry_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateCountry(null));

            Assert.That(result.ParamName, Is.EqualTo("countryViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            CountryViewModel model = CreateModel();
            await sut.CreateCountry(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateCountryCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(false);

            CountryViewModel model = CreateModel();
            IActionResult result = await sut.CreateCountry(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreateCountry()
        {
            Controller sut = CreateSut(false);

            CountryViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateCountry(model);

            Assert.That(result.ViewName, Is.EqualTo("CreateCountry"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(false);

            CountryViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateCountry(model);

            Assert.That(result.Model, Is.TypeOf<CountryViewModel>());

            CountryViewModel countryViewModel = (CountryViewModel) result.Model;

            Assert.That(countryViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            await sut.CreateCountry(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateCountryCommand>(command =>
                    string.CompareOrdinal(command.CountryCode, model.Code.ToUpper()) == 0 &&
                    string.CompareOrdinal(command.Name, model.Name) == 0 &&
                    string.CompareOrdinal(command.UniversalName, model.UniversalName) == 0 &&
                    string.CompareOrdinal(command.PhonePrefix, model.PhonePrefix) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            IActionResult result = await sut.CreateCountry(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateCountry(model);

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateCountry_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToCountries()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateCountry(model);

            Assert.That(result.ActionName, Is.EqualTo("Countries"));
        }

        private Controller CreateSut(bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateCountryCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private CountryViewModel CreateModel()
        {
            return _fixture.Create<CountryViewModel>();
        }
    }
}