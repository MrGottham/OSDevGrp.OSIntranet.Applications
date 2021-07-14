using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class PostingLinesAsyncTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            
            _fixture = new Fixture();
            _fixture.Customize<IPostingLine>(builder => builder.FromFactory(() => _fixture.BuildPostingLineMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalledWithoutStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.PostingLinesAsync(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostingLineCollectionQuery, IPostingLineCollection>(It.Is<IGetPostingLineCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today && value.NumberOfPostingLines > 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalledWithStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.PostingLinesAsync(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostingLineCollectionQuery, IPostingLineCollection>(It.Is<IGetPostingLineCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == statusDate.Date && value.NumberOfPostingLines > 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalledWithoutNumberOfPostingLines_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.PostingLinesAsync(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostingLineCollectionQuery, IPostingLineCollection>(It.Is<IGetPostingLineCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today && value.NumberOfPostingLines == 25)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalledWithNumberOfPostingLines_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            int numberOfPostingLines = _fixture.Create<int>();
            await sut.PostingLinesAsync(accountingNumber, numberOfPostingLines: numberOfPostingLines);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostingLineCollectionQuery, IPostingLineCollection>(It.Is<IGetPostingLineCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today && value.NumberOfPostingLines == numberOfPostingLines)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<PostingLineCollectionModel> result = await sut.PostingLinesAsync(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalled_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<PostingLineCollectionModel> result = await sut.PostingLinesAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<PostingLineCollectionModel> result = await sut.PostingLinesAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.PostingLinesAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsPostingLineCollectionModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.PostingLinesAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.TypeOf<PostingLineCollectionModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostingLinesAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsPostingLineCollectionModelContainingAllDebtorAccounts()
        {
            IList<IPostingLine> postingLines = _fixture.CreateMany<IPostingLine>(_random.Next(5, 10)).ToList();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLines).Object;
            Controller sut = CreateSut(postingLineCollection);

            OkObjectResult result = (OkObjectResult) (await sut.PostingLinesAsync(_fixture.Create<int>())).Result;

            PostingLineCollectionModel postingLineCollectionModel = (PostingLineCollectionModel) result.Value;
            Assert.That(postingLineCollectionModel, Is.Not.Null);
            Assert.That(postingLineCollectionModel.Count, Is.EqualTo(postingLines.Count));
            Assert.That(postingLineCollectionModel.All(postingLineModel => postingLines.SingleOrDefault(postingLine => postingLineModel.Identifier != null && postingLineModel.Identifier.Value == postingLine.Identifier) != null), Is.True);
        }

        private Controller CreateSut(IPostingLineCollection postingLineCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetPostingLineCollectionQuery, IPostingLineCollection>(It.IsAny<IGetPostingLineCollectionQuery>()))
                .Returns(Task.FromResult(postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}