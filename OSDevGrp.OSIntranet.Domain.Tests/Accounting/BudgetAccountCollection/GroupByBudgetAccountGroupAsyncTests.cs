using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountCollection
{
    [TestFixture]
    public class GroupByBudgetAccountGroupAsyncTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_AssertBudgetAccountGroupWasCalledOnEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            Mock<IBudgetAccount>[] budgetAccountMockCollection = CreateBudgetAccountMockCollection();
            sut.Add(budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray());

            await sut.GroupByBudgetAccountGroupAsync();

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.BudgetAccountGroup, Times.AtLeastOnce);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_AssertNumberWasCalledOnBudgetAccountGroupForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            Mock<IBudgetAccountGroup>[] budgetAccountGroupMockCollection =
            {
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock()
            };
            sut.Add(CreateBudgetAccountCollection(budgetAccountGroupMockCollection.Select(budgetAccountGroupMock => budgetAccountGroupMock.Object).ToArray()));

            await sut.GroupByBudgetAccountGroupAsync();

            foreach (Mock<IBudgetAccountGroup> budgetAccountGroupMock in budgetAccountGroupMockCollection)
            {
                budgetAccountGroupMock.Verify(m => m.Number, Times.AtLeastOnce);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_AssertAccountNumberWasCalledOnEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            Mock<IBudgetAccount>[] budgetAccountMockCollection = CreateBudgetAccountMockCollection();
            sut.Add(budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray());

            await sut.GroupByBudgetAccountGroupAsync();

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.AccountNumber, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(CreateBudgetAccountCollection());

            IReadOnlyDictionary<IBudgetAccountGroup, IBudgetAccountCollection> result = await sut.GroupByBudgetAccountGroupAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhichContainsEachBudgetAccountGroupFromBudgetAccountsInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IBudgetAccountGroup[] budgetAccountGroupCollection = _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(2, 5)).ToArray();
            sut.Add(CreateBudgetAccountCollection(budgetAccountGroupCollection));

            IReadOnlyDictionary<IBudgetAccountGroup, IBudgetAccountCollection> result = await sut.GroupByBudgetAccountGroupAsync();

            Assert.That(budgetAccountGroupCollection.All(budgetAccountGroup => result.Keys.Single(key => key.Number == budgetAccountGroup.Number) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhichContainsBudgetAccountCollectionMatchingEachBudgetAccountGroupFromBudgetAccountsInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(CreateBudgetAccountCollection());

            IReadOnlyDictionary<IBudgetAccountGroup, IBudgetAccountCollection> result = await sut.GroupByBudgetAccountGroupAsync();

            Assert.That(result.All(item => item.Value.All(budgetAccount => budgetAccount.BudgetAccountGroup.Number == item.Key.Number)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhichContainsAllBudgetAccountsInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IBudgetAccount[] budgetAccountCollection = CreateBudgetAccountCollection();
            sut.Add(budgetAccountCollection);

            IReadOnlyDictionary<IBudgetAccountGroup, IBudgetAccountCollection> result = await sut.GroupByBudgetAccountGroupAsync();

            Assert.That(budgetAccountCollection.All(budgetAccount => result.SelectMany(item => item.Value).Contains(budgetAccount)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhereAllBudgetAccountCollectionsIsCalculated()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(CreateBudgetAccountCollection());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IReadOnlyDictionary<IBudgetAccountGroup, IBudgetAccountCollection> result = await (await sut.CalculateAsync(statusDate)).GroupByBudgetAccountGroupAsync();

            Assert.That(result.Select(item => item.Value.StatusDate).All(value => value == statusDate.Date), Is.True);
        }

        private IBudgetAccountCollection CreateSut()
        {
            return new Domain.Accounting.BudgetAccountCollection();
        }

        private IBudgetAccount[] CreateBudgetAccountCollection(IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = null)
        {
            return CreateBudgetAccountMockCollection(budgetAccountGroupCollection)
                .Select(budgetAccountMock => budgetAccountMock.Object)
                .ToArray();
        }

        private Mock<IBudgetAccount>[] CreateBudgetAccountMockCollection(IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = null)
        {
            budgetAccountGroupCollection ??= _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(2, 5));

            return budgetAccountGroupCollection.SelectMany(budgetAccountGroup =>
                {
                    int numberOfBudgetAccounts = _random.Next(5, 10);

                    List<Mock<IBudgetAccount>> budgetAccountMockCollection = new List<Mock<IBudgetAccount>>();
                    while (budgetAccountMockCollection.Count < numberOfBudgetAccounts)
                    {
                        budgetAccountMockCollection.Add(_fixture.BuildBudgetAccountMock(budgetAccountGroup: budgetAccountGroup));
                    }

                    return budgetAccountMockCollection;
                })
                .ToArray();
        }
    }
}