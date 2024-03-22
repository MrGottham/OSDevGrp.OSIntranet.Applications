using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.ExternalData.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.ExternalData.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.HomeController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.HomeController
{
    [TestFixture]
    public class CollectNewsTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();

            _fixture = new Fixture();
            _fixture.Customize<INews>(builder => builder.FromFactory(() => _fixture.BuildNewsMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetNewsCollectionQuery()
        {
            Controller sut = CreateSut();

            await sut.CollectNews(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetNewsCollectionQuery, IEnumerable<INews>>(It.Is<IGetNewsCollectionQuery>(query => query != null && query.GetType() == typeof(GetNewsCollectionQuery))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetNewsCollectionQueryWhereFromExternalDashboardIsTrue()
        {
            Controller sut = CreateSut();

            await sut.CollectNews(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetNewsCollectionQuery, IEnumerable<INews>>(It.Is<IGetNewsCollectionQuery>(query => query != null && query.FromExternalDashboard)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetNewsCollectionQueryWhereNumberOfNewsIsEqualToNumberOfNewsToCollectFromArgument()
        {
            Controller sut = CreateSut();

            int numberOfNewsToCollect = _fixture.Create<int>();
            await sut.CollectNews(numberOfNewsToCollect);

            _queryBusMock.Verify(m => m.QueryAsync<IGetNewsCollectionQuery, IEnumerable<INews>>(It.Is<IGetNewsCollectionQuery>(query => query != null && query.NumberOfNews == numberOfNewsToCollect)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CollectNews(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenCalled_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CollectNews(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenCalled_ReturnsPartialViewResultWhereViewNameIsNotNullEmptyOrWhiteSpace()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(string.IsNullOrWhiteSpace(result.ViewName), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenCalled_ReturnsPartialViewResultWhereViewNameIsEqualToExternalNewsCollectionPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.ViewName, Is.EqualTo("_ExternalNewsCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenNonEmptyNewsCollectionHasBeenCollected_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenNonEmptyNewsCollectionHasBeenCollected_ReturnsPartialViewResultWhereModelIsExternalNewsViewModelCollection()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.TypeOf<ExternalNewsViewModel[]>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenNonEmptyNewsCollectionHasBeenCollected_ReturnsPartialViewResultWhereModelIsNotEmpty()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenNonEmptyNewsCollectionHasBeenCollected_ReturnsPartialViewResultWhereModelContainsCollectedNews()
        {
            IEnumerable<INews> newsCollection = new[]
            {
                _fixture.BuildNewsMock(_fixture.Create<string>()).Object,
                _fixture.BuildNewsMock(_fixture.Create<string>()).Object,
                _fixture.BuildNewsMock(_fixture.Create<string>()).Object,
                _fixture.BuildNewsMock(_fixture.Create<string>()).Object,
                _fixture.BuildNewsMock(_fixture.Create<string>()).Object,
                _fixture.BuildNewsMock(_fixture.Create<string>()).Object,
                _fixture.BuildNewsMock(_fixture.Create<string>()).Object
            };
            Controller sut = CreateSut(newsCollection: newsCollection);

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            IEnumerable<ExternalNewsViewModel> externalNewsViewModelCollection = (IEnumerable<ExternalNewsViewModel>) result.Model;

            Assert.That(newsCollection.All(news => externalNewsViewModelCollection!.SingleOrDefault(externalNewsViewModel => string.CompareOrdinal(externalNewsViewModel.Identifier, news.Identifier) == 0) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenEmptyNewsCollectionHasBeenCollected_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut(newsCollection: Array.Empty<INews>());

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenEmptyNewsCollectionHasBeenCollected_ReturnsPartialViewResultWhereModelIsExternalNewsViewModelCollection()
        {
            Controller sut = CreateSut(newsCollection: Array.Empty<INews>());

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.TypeOf<ExternalNewsViewModel[]>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenEmptyNewsCollectionHasBeenCollected_ReturnsPartialViewResultWhereModelIsEmpty()
        {
            Controller sut = CreateSut(newsCollection: Array.Empty<INews>());

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenNewsCollectionHasNotBeenCollected_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut(hasNews: false);

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenNewsCollectionHasNotBeenCollected_ReturnsPartialViewResultWhereModelIsExternalNewsViewModelCollection()
        {
            Controller sut = CreateSut(hasNews: false);

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.TypeOf<ExternalNewsViewModel[]>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CollectNews_WhenNewsCollectionHasNotBeenCollected_ReturnsPartialViewResultWhereModelIsEmpty()
        {
            Controller sut = CreateSut(hasNews: false);

            PartialViewResult result = (PartialViewResult) (await sut.CollectNews(_fixture.Create<int>()));

            Assert.That(result.Model, Is.Empty);
        }

        private Controller CreateSut(bool hasNews = true, IEnumerable<INews> newsCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetNewsCollectionQuery, IEnumerable<INews>>(It.IsAny<IGetNewsCollectionQuery>()))
                .Returns(Task.FromResult(hasNews ? newsCollection ?? _fixture.CreateMany<INews>(_random.Next(10, 25)) : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}