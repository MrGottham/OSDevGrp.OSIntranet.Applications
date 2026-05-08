using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class PostingJournalAsyncTests
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
        public async Task PostingJournalAsync_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetUserSpecificKeyQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.PostingJournalAsync(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Count() == 2 && string.CompareOrdinal(query.KeyElementCollection.ElementAtOrDefault(0), nameof(ApplyPostingJournalModel)) == 0 && string.CompareOrdinal(query.KeyElementCollection.ElementAt(1), Convert.ToString(accountingNumber)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingJournalAsync_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQuery()
        {
            string postingJournalKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalKey: postingJournalKey);

            await sut.PostingJournalAsync(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingJournalAsync_WhenKeyValueEntryWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntry()
        {
            Mock<IKeyValueEntry> keyValueEntryMock = _fixture.BuildKeyValueEntryMock<ApplyPostingJournalModel>();
            Controller sut = CreateSut(keyValueEntry: keyValueEntryMock.Object);

            await sut.PostingJournalAsync(_fixture.Create<int>());

            keyValueEntryMock.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingJournalAsync_WhenNoKeyValueEntryWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsApplyPostingJournalModelWithAccountingNumberEqualToArgument()
        {
            Controller sut = CreateSut(hasKeyValueEntry: false);

            int accountingNumber = _fixture.Create<int>();
            OkObjectResult result = (OkObjectResult)(await sut.PostingJournalAsync(accountingNumber)).Result;

            ApplyPostingJournalModel postingJournalModel = (ApplyPostingJournalModel)result.Value;

            Assert.That(postingJournalModel, Is.Not.Null);
            Assert.That(postingJournalModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingJournalAsync_WhenNoKeyValueEntryWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsApplyPostingJournalModelWithApplyPostingLinesNotNull()
        {
            Controller sut = CreateSut(hasKeyValueEntry: false);

            OkObjectResult result = (OkObjectResult)(await sut.PostingJournalAsync(_fixture.Create<int>())).Result;

            ApplyPostingJournalModel postingJournalModel = (ApplyPostingJournalModel)result.Value;

            Assert.That(postingJournalModel, Is.Not.Null);
            Assert.That(postingJournalModel.ApplyPostingLines, Is.Not.Null);
            Assert.That(postingJournalModel.ApplyPostingLines, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingJournalAsync_WhenKeyValueEntryWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsApplyPostingJournalModelFromKeyValueEntry()
        {
            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalModel postingJournalModel = new ApplyPostingJournalModel
            {
                AccountingNumber = accountingNumber,
                ApplyPostingLines = new ApplyPostingLineCollectionModel
                {
                    CreateApplyPostingLineModel(Guid.NewGuid(), DateTimeOffset.Now.AddDays(-1), 1),
                    CreateApplyPostingLineModel(Guid.NewGuid(), DateTimeOffset.Now, 2)
                }
            };
            Controller sut = CreateSut(keyValueEntry: _fixture.BuildKeyValueEntryMock<ApplyPostingJournalModel>(toObject: postingJournalModel).Object);

            OkObjectResult result = (OkObjectResult)(await sut.PostingJournalAsync(_fixture.Create<int>())).Result;

            ApplyPostingJournalModel value = (ApplyPostingJournalModel)result.Value;

            Assert.That(value, Is.Not.Null);
            Assert.That(value.AccountingNumber, Is.EqualTo(postingJournalModel.AccountingNumber));
            Assert.That(value.ApplyPostingLines.Count, Is.EqualTo(postingJournalModel.ApplyPostingLines.Count));
            Assert.That(value.ApplyPostingLines.Select(model => model.Identifier), Is.EqualTo(postingJournalModel.ApplyPostingLines.Select(model => model.Identifier)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingJournalAsync_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<ApplyPostingJournalModel> result = await sut.PostingJournalAsync(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        private Controller CreateSut(string postingJournalKey = null, bool hasKeyValueEntry = true, IKeyValueEntry keyValueEntry = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.IsAny<IGetUserSpecificKeyQuery>()))
                .Returns(Task.FromResult(postingJournalKey ?? _fixture.Create<string>()));
            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.IsAny<IPullKeyValueEntryQuery>()))
                .Returns(Task.FromResult(hasKeyValueEntry ? keyValueEntry ?? _fixture.BuildKeyValueEntryMock<ApplyPostingJournalModel>().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, ConverterFactoryCreator.Create());
        }

        private static ApplyPostingLineModel CreateApplyPostingLineModel(Guid identifier, DateTimeOffset postingDate, int sortOrder)
        {
            return new ApplyPostingLineModel
            {
                Identifier = identifier,
                PostingDate = postingDate,
                AccountNumber = "1000",
                Details = "Test",
                Debit = 100M,
                SortOrder = sortOrder
            };
        }
    }
}