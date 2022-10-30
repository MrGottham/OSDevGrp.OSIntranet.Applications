using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.MonthlyAccountingStatementToMarkdownConverter
{
    [TestFixture]
    public class ConvertAsyncTests
    {
        #region Private variables

        private Mock<IStatusDateProvider> _statusDateProviderMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _statusDateProviderMock = new Mock<IStatusDateProvider>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ConvertAsync_WhenAccountingIsNull_ThrowsArgumentNullException()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForMonthOfStatusDateWasCalledFourTimesOnEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus>[] budgetAccountGroupStatusMockCollection =
            {
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock()
            };
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetAccountGroupStatusMockCollection.Select(m => m.Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock in budgetAccountGroupStatusMockCollection)
            {
                budgetAccountGroupStatusMock.Verify(m => m.ValuesForMonthOfStatusDate, Times.Exactly(4));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledTwiceOnValuesForMonthOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledTwiceOnValuesForMonthOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForLastMonthOfStatusDateWasCalledFourTimesOnEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus>[] budgetAccountGroupStatusMockCollection =
            {
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock(),
                _fixture.BuildBudgetAccountGroupStatusMock()
            };
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetAccountGroupStatusMockCollection.Select(m => m.Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock in budgetAccountGroupStatusMockCollection)
            {
                budgetAccountGroupStatusMock.Verify(m => m.ValuesForLastMonthOfStatusDate, Times.Exactly(4));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledTwiceOnValuesForLastMonthOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledTwiceOnValuesForLastMonthOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForMonthOfStatusDateWasCalledTwiceOnEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetAccount>[] budgetAccountMockCollection =
            {
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock()
            };
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray()).Object).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: new[] { budgetAccountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.ValuesForMonthOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledOnValuesForMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: new[] { budgetAccountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledOnValuesForMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: new[] { budgetAccountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForLastMonthOfStatusDateWasCalledTwiceOnEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetAccount>[] budgetAccountMockCollection =
            {
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock()
            };
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray()).Object).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: new[] { budgetAccountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.ValuesForLastMonthOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledOnValuesForLastMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: new[] { budgetAccountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledOnValuesForLastMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetInfoValues>[] budgetInfoValuesMockCollection =
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: new[] { budgetAccountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotNull()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotEmpty()
        {
            IMonthlyAccountingStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Empty);
        }

        private IMonthlyAccountingStatementToMarkdownConverter CreateSut()
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(DateTime.Today.AddDays(_random.Next(0, 365) * -1));

            _claimResolverMock.Setup(m => m.GetName())
                .Returns(_fixture.Create<string>());
            _claimResolverMock.Setup(m => m.GetMailAddress())
                .Returns($"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local");

            return new BusinessLogic.Accounting.Logic.MonthlyAccountingStatementToMarkdownConverter(_statusDateProviderMock.Object, _claimResolverMock.Object);
        }

        private IAccounting CreateAccountingWithBudgetAccountGroupStatusCollection()
        {
            IBudgetAccountGroupStatus[] budgetAccountGroupStatusCollection =
            {
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object
            };
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetAccountGroupStatusCollection).Object;
            return _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
        }
    }
}