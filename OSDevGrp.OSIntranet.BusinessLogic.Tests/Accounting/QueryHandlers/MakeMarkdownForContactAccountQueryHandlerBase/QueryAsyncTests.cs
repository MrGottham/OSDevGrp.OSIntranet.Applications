using System;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.MakeMarkdownForContactAccountQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IAccountToMarkdownConverter<IContactAccount>> _contactAccountToMarkdownConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _statusDateSetterMock = new Mock<IStatusDateSetter>();
            _contactAccountToMarkdownConverterMock = new Mock<IAccountToMarkdownConverter<IContactAccount>>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportFromAccountQuery()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = CreateExportFromAccountQueryMock();
            await sut.QueryAsync(exportFromAccountQueryMock.Object);

            exportFromAccountQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountNumberWasCalledOnExportFromAccountQuery()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = CreateExportFromAccountQueryMock();
            await sut.QueryAsync(exportFromAccountQueryMock.Object);

            exportFromAccountQueryMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportFromAccountQuery()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = CreateExportFromAccountQueryMock();
            await sut.QueryAsync(exportFromAccountQueryMock.Object);

            exportFromAccountQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetContactAccountAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery(accountingNumber, accountNumber, statusDate);
            await sut.QueryAsync(exportFromAccountQuery);

            _accountingRepositoryMock.Verify(m => m.GetContactAccountAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<string>(value => string.CompareOrdinal(value, accountNumber) == 0),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountWasReturned_AssertCalculateAsyncWasCalledOnContactAccount()
        {
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(contactAccount: contactAccountMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery(statusDate: statusDate);
            await sut.QueryAsync(exportFromAccountQuery);

            contactAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedContactAccountWasReturnedFromContactAccount_AssertConvertAsyncWasCalledOnContactAccountToMarkdownConverterWithCalculatedContactAccount()
        {
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock().Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(calculatedContactAccount: calculatedContactAccount).Object;
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(contactAccount: contactAccount);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            await sut.QueryAsync(exportFromAccountQuery);

            _contactAccountToMarkdownConverterMock.Verify(m => m.ConvertAsync(It.Is<IContactAccount>(value => value != null && value == calculatedContactAccount)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountWasReturned_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedContactAccountWasReturnedFromContactAccount_ReturnsNotNull()
        {
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(hasCalculatedContactAccount: false).Object;
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(contactAccount: contactAccount);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedContactAccountWasReturnedFromContactAccount_ReturnsEmptyByteArray()
        {
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(hasCalculatedContactAccount: false).Object;
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(contactAccount: contactAccount);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownWasReturnedForCalculatedContactAccount_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownWasReturnedForCalculatedContactAccount_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedContactAccount_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedContactAccount_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedContactAccount_ReturnsByteArrayForMarkdownContent()
        {
            string markdownContent = _fixture.Create<string>();
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(markdownContent: markdownContent);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(markdownContent));
        }

        private IQueryHandler<IExportFromAccountQuery, byte[]> CreateSut(bool hasContactAccount = true, IContactAccount contactAccount = null, bool hasMarkdownContent = true, string markdownContent = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetContactAccountAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasContactAccount ? contactAccount ?? _fixture.BuildContactAccountMock().Object : null));

            _contactAccountToMarkdownConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IContactAccount>()))
                .Returns(Task.FromResult(hasMarkdownContent ? markdownContent ?? _fixture.Create<string>() : null));

            return new MyMakeMarkdownForContactAccountQueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _contactAccountToMarkdownConverterMock.Object, false);
        }

        private IExportFromAccountQuery CreateExportFromAccountQuery(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            return CreateExportFromAccountQueryMock(accountingNumber, accountNumber, statusDate).Object;
        }

        private Mock<IExportFromAccountQuery> CreateExportFromAccountQueryMock(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = new Mock<IExportFromAccountQuery>();
            exportFromAccountQueryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportFromAccountQueryMock.Setup(m => m.AccountNumber)
                .Returns(accountNumber ?? _fixture.Create<string>());
            exportFromAccountQueryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportFromAccountQueryMock;
        }

        private class MyMakeMarkdownForContactAccountQueryHandler : MakeMarkdownForContactAccountQueryHandlerBase<IExportFromAccountQuery, IAccountToMarkdownConverter<IContactAccount>>
        {
            #region Constructor

            public MyMakeMarkdownForContactAccountQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IAccountToMarkdownConverter<IContactAccount> contactAccountToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true)
                : base(validator, accountingRepository, statusDateSetter, contactAccountToMarkdownConverter, encoderShouldEmitUtf8Identifier)
            {
            }

            #endregion
        }
    }
}