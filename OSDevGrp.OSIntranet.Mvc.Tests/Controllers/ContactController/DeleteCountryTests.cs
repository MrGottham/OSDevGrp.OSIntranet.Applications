using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class DeleteCountryTests
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
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteCountry_WhenCalledWithCodeWhereCodeIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountry(null));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteCountry_WhenCalledWithCodeWhereCodeIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountry(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteCountry_WhenCalledWithCodeWhereCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountry(" "));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteCountry_WhenCalledWithCode_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            string code = _fixture.Create<string>();
            await sut.DeleteCountry(code);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteCountryCommand>(value => string.CompareOrdinal(value.CountryCode, code.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteCountry_WhenCalledWithCode_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteCountry(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteCountry_WhenCalledWithCode_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteCountry(_fixture.Create<string>());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteCountry_WhenCalledWithCode_ReturnsRedirectToActionResultWhereActionNameIsEqualToCountries()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteCountry(_fixture.Create<string>());

            Assert.That(result.ActionName, Is.EqualTo("Countries"));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteCountryCommand>()))
                .Returns(Task.Run(() => { }));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
