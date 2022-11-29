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
    public class ExportContactAccountCollectionToCsvTests
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
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasNotGiven_AssertQueryAsyncWasCalledOnQueryBusWithExportContactAccountCollectionQueryWhereStatusDateIsEqualToToday()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.ExportContactAccountCollectionToCsv(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IExportContactAccountCollectionQuery, byte[]>(It.Is<IExportContactAccountCollectionQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithTimestamp_AssertQueryAsyncWasCalledOnQueryBusWithExportContactAccountCollectionQueryWhereStatusDateIsEqualToDateOfGivenStatusDate()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            await sut.ExportContactAccountCollectionToCsv(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IExportContactAccountCollectionQuery, byte[]>(It.Is<IExportContactAccountCollectionQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithoutTimestamp_AssertQueryAsyncWasCalledOnQueryBusWithExportContactAccountCollectionQueryWhereStatusDateIsEqualToDateOfGivenStatusDate()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            await sut.ExportContactAccountCollectionToCsv(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IExportContactAccountCollectionQuery, byte[]>(It.Is<IExportContactAccountCollectionQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenNoCsvContentWasReturnedFromQueryBus_ReturnNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenNoCsvContentWasReturnedFromQueryBus_ReturnFileContentResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenNoCsvContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut(false);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenNoCsvContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsEmpty()
        {
            Controller sut = CreateSut(false);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCsvContentWasReturnedFromQueryBus_ReturnNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCsvContentWasReturnedFromQueryBus_ReturnFileContentResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCsvContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCsvContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsNotEmpty()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCsvContentWasReturnedFromQueryBus_ReturnFileContentResultWhereFileContentsIsEqualToCsvContentFromQueryBus()
        {
            byte[] csvContent = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            Controller sut = CreateSut(csvContent: csvContent);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileContents, Is.EqualTo(csvContent));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCalled_ReturnFileContentResultWhereContentTypeIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.ContentType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCalled_ReturnFileContentResultWhereContentTypeIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.ContentType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenCalled_ReturnFileContentResultWhereContentTypeIsEqualToApplicationCsv()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.ContentType, Is.EqualTo("application/csv"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasNotGiven_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForContactAccountCollectionPrefixedWithToday()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>());

            Assert.That(result.FileDownloadName, Is.EqualTo($"{DateTime.Today:yyyyMMdd} - Contact accounts.csv"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithTimestamp_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForContactAccountCollectionPrefixedWithStatusDate()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(1, 12 * 60));
            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.EqualTo($"{statusDate:yyyyMMdd} - Contact accounts.csv"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContactAccountCollectionToCsv_WhenStatusDateWasGivenWithoutTimestamp_ReturnFileContentResultWhereFileDownloadNameIsEqualToFileNameForContactAccountCollectionPrefixedWithStatusDate()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            FileContentResult result = (FileContentResult)await sut.ExportContactAccountCollectionToCsv(_fixture.Create<int>(), statusDate);

            Assert.That(result.FileDownloadName, Is.EqualTo($"{statusDate:yyyyMMdd} - Contact accounts.csv"));
        }

        private Controller CreateSut(bool hasCsvContent = true, byte[] csvContent = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IExportContactAccountCollectionQuery, byte[]>(It.IsAny<IExportContactAccountCollectionQuery>()))
                .Returns(Task.FromResult(hasCsvContent ? csvContent ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}