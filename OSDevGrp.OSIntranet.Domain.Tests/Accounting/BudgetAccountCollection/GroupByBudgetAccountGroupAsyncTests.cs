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
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_AssertCalculateAsyncWasCalledOnEachUniqueBudgetAccountGroupInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            Mock<IBudgetAccountGroup>[] budgetAccountGroupMockCollection =
            {
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock()
            };
            sut.Add(CreateBudgetAccountCollection(budgetAccountGroupMockCollection.Select(budgetAccountGroupMock => budgetAccountGroupMock.Object).ToArray()));

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 365) * -1);
            await sut.CalculateAsync(statusDate);

            await sut.GroupByBudgetAccountGroupAsync();

            foreach (Mock<IBudgetAccountGroup> budgetAccountGroupMock in budgetAccountGroupMockCollection)
            {
                budgetAccountGroupMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date), It.Is<IBudgetAccountCollection>(value => value != null)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(CreateBudgetAccountCollection());

            IEnumerable<IBudgetAccountGroupStatus> result = await sut.GroupByBudgetAccountGroupAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsNotEmpty()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(CreateBudgetAccountCollection());

            IEnumerable<IBudgetAccountGroupStatus> result = await sut.GroupByBudgetAccountGroupAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByBudgetAccountGroupAsync_WhenCalled_ReturnsBudgetAccountGroupStatusCollectionWhichContainsCalculatedBudgetAccountGroupStatusFromEachUniqueBudgetAccountGroupInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            int budgetAccountGroupNumber1 = _fixture.Create<int>();
            int budgetAccountGroupNumber2 = _fixture.Create<int>();
            int budgetAccountGroupNumber3 = _fixture.Create<int>();
            IBudgetAccountGroupStatus[] calculatedBudgetAccountGroupStatusCollection =
            {
                _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountGroupNumber1).Object,
                _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountGroupNumber2).Object,
                _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountGroupNumber3).Object
            };

            IBudgetAccountGroup[] budgetAccountGroupCollection =
            {
                _fixture.BuildBudgetAccountGroupMock(budgetAccountGroupNumber1, calculatedBudgetAccountGroupStatus: calculatedBudgetAccountGroupStatusCollection[0]).Object,
                _fixture.BuildBudgetAccountGroupMock(budgetAccountGroupNumber2, calculatedBudgetAccountGroupStatus: calculatedBudgetAccountGroupStatusCollection[1]).Object,
                _fixture.BuildBudgetAccountGroupMock(budgetAccountGroupNumber3, calculatedBudgetAccountGroupStatus: calculatedBudgetAccountGroupStatusCollection[2]).Object
            };
            sut.Add(CreateBudgetAccountCollection(budgetAccountGroupCollection));

            IEnumerable<IBudgetAccountGroupStatus> result = await sut.GroupByBudgetAccountGroupAsync();

            Assert.That(calculatedBudgetAccountGroupStatusCollection.All(calculatedBudgetAccountGroupStatus => result.Contains(calculatedBudgetAccountGroupStatus)), Is.True);
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
            budgetAccountGroupCollection ??= new[]
            {
                _fixture.BuildBudgetAccountGroupMock().Object,
                _fixture.BuildBudgetAccountGroupMock().Object,
                _fixture.BuildBudgetAccountGroupMock().Object
            };

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