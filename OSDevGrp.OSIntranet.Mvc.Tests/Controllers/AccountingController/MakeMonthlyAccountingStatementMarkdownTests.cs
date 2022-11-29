﻿using System;
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
    public class MakeMonthlyAccountingStatementMarkdownTests
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
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasNotGiven_AssertQueryAsyncWasCalledOnQueryBusWithMakeMonthlyAccountingStatementQueryWhereStatusDateIsEqualToToday()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.MakeMonthlyAccountingStatementMarkdown(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IMakeMonthlyAccountingStatementQuery, byte[]>(It.Is<IMakeMonthlyAccountingStatementQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_AssertQueryAsyncWasCalledOnQueryBusWithMakeMonthlyAccountingStatementQueryWhereStatusDateIsEqualToDateOfGivenStatusDate()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            await sut.MakeMonthlyAccountingStatementMarkdown(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IMakeMonthlyAccountingStatementQuery, byte[]>(It.Is<IMakeMonthlyAccountingStatementQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_AssertQueryAsyncWasCalledOnQueryBusWithMakeMonthlyAccountingStatementQueryWhereStatusDateIsEqualToDateOfGivenStatusDate()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            await sut.MakeMonthlyAccountingStatementMarkdown(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IMakeMonthlyAccountingStatementQuery, byte[]>(It.Is<IMakeMonthlyAccountingStatementQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut(false);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenNoMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsEmpty()
        {
            Controller sut = CreateSut(false);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotEmpty()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenMarkdownContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsEqualToMarkdownContentFromQueryBus()
        {
            byte[] markdownContent = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            Controller sut = CreateSut(markdownContent: markdownContent);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.EqualTo(markdownContent));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenCalled_ReturnFileContentResultWhereContentTypeIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.ContentType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenCalled_ReturnFileContentResultWhereContentTypeIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.ContentType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenCalled_ReturnFileContentResultWhereContentTypeIsEqualToTextMarkdown()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.ContentType, Is.EqualTo("text/markdown"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForMonthlyResultPrefixedWithToday()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>());

            Assert.That(result.FileDownloadName, Is.EqualTo($"{DateTime.Today:yyyyMMdd} - Monthly result.md"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForMonthlyResultPrefixedWithStatusDate()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.EqualTo($"{statusDate:yyyyMMdd} - Monthly result.md"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MakeMonthlyAccountingStatementMarkdown_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForMonthlyResultPrefixedWithStatusDate()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.MakeMonthlyAccountingStatementMarkdown(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.EqualTo($"{statusDate:yyyyMMdd} - Monthly result.md"));
        }

        private Controller CreateSut(bool hasMakeContent = true, byte[] markdownContent = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IMakeMonthlyAccountingStatementQuery, byte[]>(It.IsAny<IMakeMonthlyAccountingStatementQuery>()))
                .Returns(Task.FromResult(hasMakeContent ? markdownContent ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}