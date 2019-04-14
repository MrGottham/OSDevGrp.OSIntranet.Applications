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
    public class AccountGroupsTests
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
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroups_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.AccountGroups();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroups_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AccountGroups();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroups_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToAccountGroups()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.AccountGroups();

            Assert.That(result.ViewName, Is.EqualTo("AccountGroups"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroups_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfAccountGroupViewModel()
        {
            IEnumerable<IAccountGroup> accountGroupMockCollection = _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(accountGroupMockCollection);

            ViewResult result = (ViewResult) await sut.AccountGroups();

            Assert.That(result.Model, Is.TypeOf<List<AccountGroupViewModel>>());

            List<AccountGroupViewModel> accountGroupViewModelCollection = ((List<AccountGroupViewModel>) result.Model);

            Assert.That(accountGroupViewModelCollection, Is.Not.Null);
            Assert.That(accountGroupViewModelCollection, Is.Not.Empty);
            Assert.That(accountGroupViewModelCollection.Count, Is.EqualTo(accountGroupMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<IAccountGroup> accountGroupCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => accountGroupCollection ?? _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToList()));

            return new Controller(_queryBusMock.Object);
        }
    }
}