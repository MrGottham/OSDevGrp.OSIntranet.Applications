using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.UpdateContactCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            IUpdateContactCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null, _accountingRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IUpdateContactCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(_contactRepositoryMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContact()
        {
            IUpdateContactCommand sut = CreateSut();

            IContact result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            Assert.That(result, Is.TypeOf<Contact>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithoutInternalIdentifier()
        {
            IUpdateContactCommand sut = CreateSut();

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).InternalIdentifier;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithExternalIdentifierFromCommand()
        {
            string externalIdentifier = _fixture.Create<string>();
            IUpdateContactCommand sut = CreateSut(externalIdentifier);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).ExternalIdentifier;

            Assert.That(result, Is.EqualTo(externalIdentifier));
        }

        private IUpdateContactCommand CreateSut(string externalIdentifier = null)
        {
            _contactRepositoryMock.Setup(m => m.GetContactGroupAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => _fixture.BuildContactGroupMock().Object));
            _accountingRepositoryMock.Setup(m => m.GetPaymentTermAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => _fixture.BuildPaymentTermMock().Object));

            return _fixture.Build<BusinessLogic.Contacts.Commands.UpdateContactCommand>()
                .With(m => m.ExternalIdentifier, externalIdentifier ?? _fixture.Create<string>())
                .With(m => m.Name, CreateNameCommandMock().Object)
                .With(m => m.Address, CreateAddressCommandMock().Object)
                .With(m => m.Company, CreateCompanyCommandMock().Object)
                .Create();
        }

        private Mock<INameCommand> CreateNameCommandMock()
        {
            Mock<INameCommand> nameCommandMock = new Mock<INameCommand>();
            nameCommandMock.Setup(m => m.ToDomain())
                .Returns(_fixture.BuildNameMock().Object);
            return nameCommandMock;
        }

        private Mock<IAddressCommand> CreateAddressCommandMock()
        {
            Mock<IAddressCommand> addressCommandMock = new Mock<IAddressCommand>();
            addressCommandMock.Setup(m => m.IsEmpty())
                .Returns(_fixture.Create<bool>());
            addressCommandMock.Setup(m => m.ToDomain())
                .Returns(_fixture.BuildAddressMock().Object);
            return addressCommandMock;
        }

        private Mock<ICompanyCommand> CreateCompanyCommandMock()
        {
            Mock<ICompanyCommand> companyCommandMock = new Mock<ICompanyCommand>();
            companyCommandMock.Setup(m => m.ToDomain())
                .Returns(_fixture.BuildCompanyMock().Object);
            return companyCommandMock;
        }
    }
}