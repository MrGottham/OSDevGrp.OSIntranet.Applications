using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class BudgetAccountGroupsTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroups_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.BudgetAccountGroups();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroups_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.BudgetAccountGroups();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroups_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToBudgetAccountGroups()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.BudgetAccountGroups();

            Assert.That(result.ViewName, Is.EqualTo("BudgetAccountGroups"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroups_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfBudgetAccountGroupViewModel()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroupMockCollection = _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(budgetAccountGroupMockCollection);

            ViewResult result = (ViewResult) await sut.BudgetAccountGroups();

            Assert.That(result.Model, Is.TypeOf<List<BudgetAccountGroupViewModel>>());

            List<BudgetAccountGroupViewModel> budgetAccountGroupViewModelCollection = ((List<BudgetAccountGroupViewModel>) result.Model);

            Assert.That(budgetAccountGroupViewModelCollection, Is.Not.Null);
            Assert.That(budgetAccountGroupViewModelCollection, Is.Not.Empty);
            Assert.That(budgetAccountGroupViewModelCollection.Count, Is.EqualTo(budgetAccountGroupMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => budgetAccountGroupCollection ?? _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToList()));

            return new Controller(_queryBusMock.Object);
        }
    }
}