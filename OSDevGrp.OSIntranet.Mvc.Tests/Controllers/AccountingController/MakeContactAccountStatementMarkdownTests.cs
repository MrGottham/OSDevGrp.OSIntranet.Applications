using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class MakeContactAccountStatementMarkdownTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenAccountNumberIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenAccountNumberIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenAccountNumberIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenAccountNumberIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenAccountNumberIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenAccountNumberIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), " ");

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasNotGiven_AssertQueryAsyncWasCalledOnQueryBusWithMakeContactAccountStatementQueryWhereStatusDateIsEqualToToday()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            await sut.MakeContactAccountStatementMarkdown(accountingNumber, accountNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IMakeContactAccountStatementQuery, byte[]>(It.Is<IMakeContactAccountStatementQuery>(value => value != null && value.AccountingNumber == accountingNumber && string.CompareOrdinal(value.AccountNumber, accountNumber.ToUpper()) == 0 && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_AssertQueryAsyncWasCalledOnQueryBusWithMakeContactAccountStatementQueryWhereStatusDateIsEqualToDateOfGivenStatusDate()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            await sut.MakeContactAccountStatementMarkdown(accountingNumber, accountNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IMakeContactAccountStatementQuery, byte[]>(It.Is<IMakeContactAccountStatementQuery>(value => value != null && value.AccountingNumber == accountingNumber && string.CompareOrdinal(value.AccountNumber, accountNumber.ToUpper()) == 0 && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_AssertQueryAsyncWasCalledOnQueryBusWithMakeContactAccountStatementQueryWhereStatusDateIsEqualToDateOfGivenStatusDate()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            await sut.MakeContactAccountStatementMarkdown(accountingNumber, accountNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IMakeContactAccountStatementQuery, byte[]>(It.Is<IMakeContactAccountStatementQuery>(value => value != null && value.AccountingNumber == accountingNumber && string.CompareOrdinal(value.AccountNumber, accountNumber.ToUpper()) == 0 && value.StatusDate == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut(false);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsEmpty()
        {
            Controller sut = CreateSut(false);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.FileContents, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotEmpty()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsEqualToMarkdownContentFromQueryBus()
        {
            byte[] markdownContent = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            Controller sut = CreateSut(markdownContent: markdownContent);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.FileContents, Is.EqualTo(markdownContent));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenCalled_ReturnFileContentResultWhereContentTypeIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ContentType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenCalled_ReturnFileContentResultWhereContentTypeIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ContentType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenCalled_ReturnFileContentResultWhereContentTypeIsEqualToTextMarkdown()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ContentType, Is.EqualTo("text/markdown"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForContactAccountStatementPrefixedWithToday()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            string accountNumber = _fixture.Create<string>();
            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), accountNumber);

            Assert.That(result.FileDownloadName, Is.EqualTo($"{DateTime.Today:yyyyMMdd} - Contact account statement ({accountNumber}).md"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForContactAccountStatementPrefixedWithStatusDate()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), accountNumber, statusDate);

            Assert.That(result.FileDownloadName, Is.EqualTo($"{statusDate:yyyyMMdd} - Contact account statement ({accountNumber}).md"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), _fixture.Create<string>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeContactAccountStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForContactAccountStatementPrefixedWithStatusDate()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.MakeContactAccountStatementMarkdown(_fixture.Create<int>(), accountNumber, statusDate);

            Assert.That(result.FileDownloadName, Is.EqualTo($"{statusDate:yyyyMMdd} - Contact account statement ({accountNumber}).md"));
        }

        private Controller CreateSut(bool hasMakeContent = true, byte[] markdownContent = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IMakeContactAccountStatementQuery, byte[]>(It.IsAny<IMakeContactAccountStatementQuery>()))
                .Returns(Task.FromResult(hasMakeContent ? markdownContent ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}