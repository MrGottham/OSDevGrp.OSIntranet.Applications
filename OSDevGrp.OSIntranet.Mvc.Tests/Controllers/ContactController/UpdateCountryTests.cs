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
    public class UpdateCountryTests
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
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateCountry_WhenCalledWithCodeWhereCodeIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateCountry((string) null));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateCountry_WhenCalledWithCodeWhereCodeIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateCountry(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateCountry_WhenCalledWithCodeWhereCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateCountry(" "));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithCode_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            string code = _fixture.Create<string>();
            await sut.UpdateCountry(code);

            _queryBusMock.Verify(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.Is<IGetCountryQuery>(value => string.CompareOrdinal(value.CountryCode, code.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithCode_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateCountry(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithCode_ReturnsViewResultWhereViewNameIsEqualToUpdateCountry()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateCountry(_fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateCountry"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithCode_ReturnsViewResultWhereModelIsCountryViewModel()
        {
            string code = _fixture.Create<string>();
            string name = _fixture.Create<string>();
            string universalName = _fixture.Create<string>();
            string phonePrefix = _fixture.Create<string>();
            ICountry country = _fixture.BuildCountryMock(code, name, universalName, phonePrefix).Object;
            Controller sut = CreateSut(country);

            ViewResult result = (ViewResult) await sut.UpdateCountry(_fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<CountryViewModel>());

            CountryViewModel countryViewModel = (CountryViewModel) result.Model;

            Assert.That(countryViewModel, Is.Not.Null);
            Assert.That(countryViewModel.Code, Is.EqualTo(code));
            Assert.That(countryViewModel.Name, Is.EqualTo(name));
            Assert.That(countryViewModel.UniversalName, Is.EqualTo(universalName));
            Assert.That(countryViewModel.PhonePrefix, Is.EqualTo(phonePrefix));
            Assert.That(countryViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateCountry_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateCountry((CountryViewModel) null));

            Assert.That(result.ParamName, Is.EqualTo("countryViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            CountryViewModel model = CreateModel();
            await sut.UpdateCountry(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateCountryCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            CountryViewModel model = CreateModel();
            IActionResult result = await sut.UpdateCountry(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdateCountry()
        {
            Controller sut = CreateSut(modelIsValid: false);

            CountryViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateCountry(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdateCountry"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            CountryViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateCountry(model);

            Assert.That(result.Model, Is.TypeOf<CountryViewModel>());

            CountryViewModel countryViewModel = (CountryViewModel) result.Model;

            Assert.That(countryViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            await sut.UpdateCountry(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateCountryCommand>(command => 
                    string.CompareOrdinal(command.CountryCode, model.Code.ToUpper()) == 0 &&
                    string.CompareOrdinal(command.Name, model.Name) == 0 &&
                    string.CompareOrdinal(command.UniversalName, model.UniversalName) == 0 &&
                    string.CompareOrdinal(command.PhonePrefix, model.PhonePrefix) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            IActionResult result = await sut.UpdateCountry(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateCountry(model);

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateCountry_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToCountries()
        {
            Controller sut = CreateSut();

            CountryViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateCountry(model);

            Assert.That(result.ActionName, Is.EqualTo("Countries"));
        }

        private Controller CreateSut(ICountry country = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.IsAny<IGetCountryQuery>()))
                .Returns(Task.Run(() => country ?? _fixture.BuildCountryMock().Object));

            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateCountryCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _tokenHelperFactoryMock.Object);
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