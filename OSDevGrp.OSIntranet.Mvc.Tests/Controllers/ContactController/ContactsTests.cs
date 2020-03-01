using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ContactsTests
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
        public void Contacts_WhenCalled_AssertGetCountryCodeWasCalledOnClaimResolver()
        {
            Controller sut = CreateSut();

            sut.Contacts();

            _claimResolverMock.Verify(m => m.GetCountryCode(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Contacts();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToContacts()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Contacts();

            Assert.That(result.ViewName, Is.EqualTo("Contacts"));
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenCalled_ReturnsViewResultWhereModelIsContactOptionsViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Contacts();

            Assert.That(result.Model, Is.TypeOf<ContactOptionsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenFilterIsNull_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Contacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenFilterIsEmpty_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Contacts(string.Empty);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenFilterIsWhiteSpace_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Contacts(" ");

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenFilterIsNotNullEmptyOrWhiteSpace_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsEqualToFilterFromArgument()
        {
            Controller sut = CreateSut();

            string filter = _fixture.Create<string>();
            ViewResult result = (ViewResult) sut.Contacts(filter);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.EqualTo(filter));
        }

        [Test]
        [Category("UnitTest")]
        public void Contacts_WhenCalled_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereDefaultCountryCodeIsEqualToCountryCodeFromClaimResolver()
        {
            string countryCode = _fixture.Create<string>();
            Controller sut = CreateSut(countryCode);

            ViewResult result = (ViewResult) sut.Contacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;


            Assert.That(contactOptionsViewModel.DefaultCountryCode, Is.EqualTo(countryCode));
        }

        private Controller CreateSut(string countryCode = null)
        {
            _claimResolverMock.Setup(m => m.GetCountryCode())
                .Returns(countryCode ?? _fixture.Create<string>());

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}