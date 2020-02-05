using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.CompanyCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasCompanyNameCommand_AssertToDomainWasCalledOnCompanyNameCommand()
        {
            Mock<ICompanyNameCommand> companyNameCommandMock = CreateCompanyNameCommandMock();
            ICompanyCommand sut = CreateSut(companyNameCommandMock.Object);

            sut.ToDomain();

            companyNameCommandMock.Verify(m => m.ToDomain(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveAddressCommand_AssertIsEmptyWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            ICompanyCommand sut = CreateSut(hasAddressCommand: false, addressCommand: addressCommandMock.Object);

            sut.ToDomain();

            addressCommandMock.Verify(m => m.IsEmpty(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveAddressCommand_AssertToDomainWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            ICompanyCommand sut = CreateSut(hasAddressCommand: false, addressCommand: addressCommandMock.Object);

            sut.ToDomain();

            addressCommandMock.Verify(m => m.ToDomain(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommand_AssertIsEmptyWasCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            ICompanyCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.ToDomain();

            addressCommandMock.Verify(m => m.IsEmpty(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreEmpty_AssertToDomainWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock(isEmpty: true);
            ICompanyCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.ToDomain();

            addressCommandMock.Verify(m => m.ToDomain(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreNotEmpty_AssertToDomainWasCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            ICompanyCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.ToDomain();

            addressCommandMock.Verify(m => m.ToDomain(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsCompany()
        {
            ICompanyCommand sut = CreateSut();

            ICompany result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<Company>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithValueFromToDomainOnNameCommand()
        {
            ICompanyName companyName = _fixture.BuildCompanyNameMock().Object;
            ICompanyNameCommand companyNameCommand = CreateCompanyNameCommandMock(companyName).Object;
            ICompanyCommand sut = CreateSut(companyNameCommand);

            IName result = sut.ToDomain().Name;

            Assert.That(result, Is.EqualTo(companyName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveAddressCommand_ReturnsContactWithEmptyAddress()
        {
            ICompanyCommand sut = CreateSut(hasAddressCommand: false);

            IAddress result = sut.ToDomain().Address;

            Assert.That(result.DisplayAddress, Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreEmpty_ReturnsContactWithEmptyAddress()
        {
            IAddressCommand addressCommand = CreateAddressCommandMock(true).Object;
            ICompanyCommand sut = CreateSut(addressCommand: addressCommand);

            IAddress result = sut.ToDomain().Address;

            Assert.That(result.DisplayAddress, Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreNotEmpty_ReturnsContactWithValueFromToDomainOnAddressCommand()
        {
            IAddress address = _fixture.BuildAddressMock().Object;
            IAddressCommand addressCommand = CreateAddressCommandMock(address: address).Object;
            ICompanyCommand sut = CreateSut(addressCommand: addressCommand);

            IAddress result = sut.ToDomain().Address;

            Assert.That(result, Is.EqualTo(address));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHavePrimaryPhone_ReturnsContactWithoutPrimaryPhone()
        {
            ICompanyCommand sut = CreateSut(hasPrimaryPhone: false);

            string result = sut.ToDomain().PrimaryPhone;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasPrimaryPhone_ReturnsContactWithPrimaryPhoneFromCommand()
        {
            string primaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(primaryPhone: primaryPhone);

            string result = sut.ToDomain().PrimaryPhone;

            Assert.That(result, Is.EqualTo(primaryPhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveSecondaryPhone_ReturnsContactWithoutSecondaryPhone()
        {
            ICompanyCommand sut = CreateSut(hasSecondaryPhone: false);

            string result = sut.ToDomain().SecondaryPhone;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasSecondaryPhone_ReturnsContactWithSecondaryPhoneFromCommand()
        {
            string secondaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            string result = sut.ToDomain().SecondaryPhone;

            Assert.That(result, Is.EqualTo(secondaryPhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveHomePage_ReturnsContactWithoutHomePage()
        {
            ICompanyCommand sut = CreateSut(hasHomePage: false);

            string result = sut.ToDomain().HomePage;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasHomePage_ReturnsContactWithHomePageFromCommand()
        {
            string homePage = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(homePage: homePage);

            string result = sut.ToDomain().HomePage;

            Assert.That(result, Is.EqualTo(homePage));
        }

        private ICompanyCommand CreateSut(ICompanyNameCommand companyNameCommand = null, bool hasAddressCommand = true, IAddressCommand addressCommand = null, bool hasPrimaryPhone = true, string primaryPhone = null, bool hasSecondaryPhone = true, string secondaryPhone = null, bool hasHomePage = true, string homePage = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.CompanyCommand>()
                .With(m => m.Name, companyNameCommand ?? CreateCompanyNameCommandMock().Object)
                .With(m => m.Address, hasAddressCommand ? addressCommand ?? CreateAddressCommandMock().Object : null)
                .With(m => m.PrimaryPhone, hasPrimaryPhone ? primaryPhone ?? _fixture.Create<string>() : null)
                .With(m => m.SecondaryPhone, hasSecondaryPhone ? secondaryPhone ?? _fixture.Create<string>() : null)
                .With(m => m.HomePage, hasHomePage ? homePage ?? _fixture.Create<string>() : null)
                .Create();
        }

        private Mock<ICompanyNameCommand> CreateCompanyNameCommandMock(ICompanyName companyName = null)
        {
            Mock<ICompanyNameCommand> companyNameCommandMock = new Mock<ICompanyNameCommand>();
            companyNameCommandMock.Setup(m => m.ToDomain())
                .Returns(companyName ?? _fixture.BuildCompanyNameMock().Object);
            return companyNameCommandMock;
        }

        private Mock<IAddressCommand> CreateAddressCommandMock(bool isEmpty = false, IAddress address = null)
        {
            Mock<IAddressCommand> addressCommandMock = new Mock<IAddressCommand>();
            addressCommandMock.Setup(m => m.IsEmpty())
                .Returns(isEmpty);
            addressCommandMock.Setup(m => m.ToDomain())
                .Returns(address ?? _fixture.BuildAddressMock().Object);
            return addressCommandMock;
        }
    }
}