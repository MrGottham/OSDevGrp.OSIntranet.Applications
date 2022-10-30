using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Markdown;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.PeriodicAccountingStatementToMarkdownConverterBase
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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetAccountCollectionWasCalledOnAccounting()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            await sut.ConvertAsync(accountingMock.Object);

            accountingMock.Verify(m => m.BudgetAccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGroupByBudgetAccountGroupAsyncWasCalledOnBudgetAccountCollectionFromAccounting()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollectionMock.Object).Object;
            await sut.ConvertAsync(accounting);

            budgetAccountCollectionMock.Verify(m => m.GroupByBudgetAccountGroupAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertNameWasCalledOnEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountGroupStatusMock.Verify(m => m.Name, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForMonthOfStatusDateWasCalledFourTimesOnEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
        public async Task ConvertAsync_WhenCalled_AssertBudgetAccountCollectionWasCalledOnEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountGroupStatusMock.Verify(m => m.BudgetAccountCollection, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountNumberWasCalledOnEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountMock.Verify(m => m.AccountNumber, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountNameWasCalledOnEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountMock.Verify(m => m.AccountName, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForMonthOfStatusDateWasCalledTwiceOnEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

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
        public async Task ConvertAsync_WhenCalled_AssertGetTableExplanationMarkdownWasCalledOnPeriodicAccountingStatementToMarkdownConverterBase()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

            await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(((MyPeriodicAccountingStatementToMarkdownConverter)sut).GetTableExplanationMarkdownWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetTableExplanationMarkdownWasCalledOnPeriodicAccountingStatementToMarkdownConverterBaseWithStatusDate()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut(statusDate);

            await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(((MyPeriodicAccountingStatementToMarkdownConverter)sut).GetTableExplanationMarkdownWasCalledWithStatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotNull()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotEmpty()
        {
            IPeriodicAccountingStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Empty);
        }

        private IPeriodicAccountingStatementToMarkdownConverter CreateSut(DateTime? statusDate = null)
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(statusDate?.Date ?? DateTime.Today.AddDays(_random.Next(0, 365) * -1));

            _claimResolverMock.Setup(m => m.GetName())
                .Returns(_fixture.Create<string>());
            _claimResolverMock.Setup(m => m.GetMailAddress())
                .Returns($"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local");

            return new MyPeriodicAccountingStatementToMarkdownConverter(_fixture.Create<string>(), _fixture.Create<string>(), _statusDateProviderMock.Object, _claimResolverMock.Object, CultureInfo.InvariantCulture);
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

        private class MyPeriodicAccountingStatementToMarkdownConverter : BusinessLogic.Accounting.Logic.PeriodicAccountingStatementToMarkdownConverterBase
        {
            #region Private variables

            private readonly string _header;
            private readonly string _tableExplanation;

            #endregion

            #region Constructor

            public MyPeriodicAccountingStatementToMarkdownConverter(string header, string tableExplanation, IStatusDateProvider statusDateProvider, IClaimResolver claimResolver, IFormatProvider formatProvider) 
                : base(statusDateProvider, claimResolver, formatProvider)
            {
                NullGuard.NotNullOrWhiteSpace(header, nameof(header))
                    .NotNullOrWhiteSpace(tableExplanation, nameof(tableExplanation));

                _header = header;
                _tableExplanation = tableExplanation;
            }

            #endregion

            #region Properties

            public bool GetTableExplanationMarkdownWasCalled { get; private set; }

            public DateTime GetTableExplanationMarkdownWasCalledWithStatusDate { get; private set; }

            #endregion

            #region Methods

            protected override string GetHeader(DateTime statusDate) => $"{_header} pr. {statusDate.ToString("D", FormatProvider)}";

            protected override IMarkdownBlockElement GetTableExplanationMarkdown(DateTime statusDate)
            {
                GetTableExplanationMarkdownWasCalled = true;
                GetTableExplanationMarkdownWasCalledWithStatusDate = statusDate;

                return new MarkdownParagraph(new MarkdownText(_tableExplanation));
            }

            protected override decimal GetBudgetForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus)
            {
                NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

                return budgetAccountGroupStatus.ValuesForMonthOfStatusDate.Budget;
            }

            protected override decimal GetBudgetForColumnSet1(IBudgetAccount budgetAccount)
            {
                NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

                return budgetAccount.ValuesForMonthOfStatusDate.Budget;
            }

            protected override decimal GetPostedForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus)
            {
                NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

                return budgetAccountGroupStatus.ValuesForMonthOfStatusDate.Posted;
            }

            protected override decimal GetPostedForColumnSet1(IBudgetAccount budgetAccount)
            {
                NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

                return budgetAccount.ValuesForMonthOfStatusDate.Posted;
            }

            protected override decimal GetBudgetForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus)
            {
                NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

                return budgetAccountGroupStatus.ValuesForLastMonthOfStatusDate.Budget;
            }

            protected override decimal GetBudgetForColumnSet2(IBudgetAccount budgetAccount)
            {
                NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

                return budgetAccount.ValuesForLastMonthOfStatusDate.Budget;
            }

            protected override decimal GetPostedForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus)
            {
                NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

                return budgetAccountGroupStatus.ValuesForLastMonthOfStatusDate.Posted;
            }

            protected override decimal GetPostedForColumnSet2(IBudgetAccount budgetAccount)
            {
                NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

                return budgetAccount.ValuesForLastMonthOfStatusDate.Posted;
            }

            #endregion
        }
    }
}