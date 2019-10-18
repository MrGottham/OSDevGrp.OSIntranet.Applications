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
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class UpdatePostalCodeTests
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
        public void UpdatePostalCode_WhenCalledWithCountryCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePostalCode(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdatePostalCode_WhenCalledWithCountryCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePostalCode(string.Empty, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdatePostalCode_WhenCalledWithCountryCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePostalCode(" ", _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdatePostalCode_WhenCalledWithPostalCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePostalCode(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdatePostalCode_WhenCalledWithPostalCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePostalCode(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdatePostalCode_WhenCalledWithPostalCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePostalCode(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithCountryCodeAndPostalCode_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            string postalCode = _fixture.Create<string>();
            await sut.UpdatePostalCode(countryCode, postalCode);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostalCodeQuery, IPostalCode>(It.Is<IGetPostalCodeQuery>(value => string.CompareOrdinal(value.CountryCode, countryCode.ToUpper()) == 0 && string.CompareOrdinal(value.PostalCode, postalCode) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithCountryCodeAndPostalCode_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdatePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithCountryCodeAndPostalCode_ReturnsViewResultWhereViewNameIsEqualToUpdatePostalCode()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdatePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("UpdatePostalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithCountryCodeAndPostalCode_ReturnsViewResultWhereModelIsPostalCodeViewModel()
        {
            string countryCode = _fixture.Create<string>();
            string postalCode = _fixture.Create<string>();
            string city = _fixture.Create<string>();
            string state = _fixture.Create<string>();
            ICountry country = _fixture.BuildCountryMock(countryCode).Object;
            IPostalCode postalCodeObj = _fixture.BuildPostalCodeMock(country, postalCode, city, state).Object;
            Controller sut = CreateSut(postalCodeObj);

            ViewResult result = (ViewResult) await sut.UpdatePostalCode(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<PostalCodeViewModel>());

            PostalCodeViewModel postalCodeViewModel = (PostalCodeViewModel) result.Model;

            Assert.That(postalCodeViewModel, Is.Not.Null);
            Assert.That(postalCodeViewModel.Country, Is.Not.Null);
            Assert.That(postalCodeViewModel.Country.Code, Is.EqualTo(countryCode));
            Assert.That(postalCodeViewModel.Code, Is.EqualTo(postalCode));
            Assert.That(postalCodeViewModel.City, Is.EqualTo(city));
            Assert.That(postalCodeViewModel.State, Is.EqualTo(state));
            Assert.That(postalCodeViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdatePostalCode_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePostalCode(null));

            Assert.That(result.ParamName, Is.EqualTo("postalCodeViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            await sut.UpdatePostalCode(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdatePostalCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            IActionResult result = await sut.UpdatePostalCode(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdatePostalCode()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdatePostalCode(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdatePostalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PostalCodeViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdatePostalCode(model);

            Assert.That(result.Model, Is.TypeOf<PostalCodeViewModel>());

            PostalCodeViewModel postalCodeViewModel = (PostalCodeViewModel) result.Model;

            Assert.That(postalCodeViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            await sut.UpdatePostalCode(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdatePostalCodeCommand>(command =>
                    string.CompareOrdinal(command.CountryCode, model.Country.Code.ToUpper()) == 0 &&
                    string.CompareOrdinal(command.PostalCode, model.Code) == 0 &&
                    string.CompareOrdinal(command.City, model.City) == 0 &&
                    string.CompareOrdinal(command.State, model.State) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            IActionResult result = await sut.UpdatePostalCode(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdatePostalCode(model);

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePostalCode_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToPostalCodes()
        {
            Controller sut = CreateSut();

            PostalCodeViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdatePostalCode(model);

            Assert.That(result.ActionName, Is.EqualTo("PostalCodes"));
        }

        private Controller CreateSut(IPostalCode postalCode = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetPostalCodeQuery, IPostalCode>(It.IsAny<IGetPostalCodeQuery>()))
                .Returns(Task.Run(() => postalCode ?? _fixture.BuildPostalCodeMock().Object));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdatePostalCodeCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object);
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
