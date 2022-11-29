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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.AnnualAccountingStatementToMarkdownConverter
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
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForYearToDateOfStatusDateWasCalledFourTimesOnEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountGroupStatusMock.Verify(m => m.ValuesForYearToDateOfStatusDate, Times.Exactly(4));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledTwiceOnValuesForYearToDateOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForYearToDateOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledTwiceOnValuesForYearToDateOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForYearToDateOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForLastYearOfStatusDateWasCalledFourTimesOnEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountGroupStatusMock.Verify(m => m.ValuesForLastYearOfStatusDate, Times.Exactly(4));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledTwiceOnValuesForLastYearOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastYearOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledTwiceOnValuesForLastYearOfStatusDateForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastYearOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(budgetAccountCollection: budgetAccountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesForYearToDateOfStatusDateWasCalledTwiceOnEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountMock.Verify(m => m.ValuesForYearToDateOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledOnValuesForYearToDateOfStatusDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForYearToDateOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
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
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledOnValuesForYearToDateOfStatusDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForYearToDateOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
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
        public async Task ConvertAsync_WhenCalled_AssertValuesForLastYearOfStatusDateWasCalledTwiceOnEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
                budgetAccountMock.Verify(m => m.ValuesForLastYearOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBudgetWasCalledOnValuesForLastYearOfStatusDateDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastYearOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
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
        public async Task ConvertAsync_WhenCalled_AssertPostedWasCalledOnValuesForLastYearOfStatusDateForEachBudgetAccountInBudgetAccountCollectionForEachBudgetAccountGroupStatusFromGroupByBudgetAccountGroupAsync()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

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
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastYearOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray()).Object).Object;
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
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotEmpty()
        {
            IAnnualAccountingStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithBudgetAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Empty);
        }

        private IAnnualAccountingStatementToMarkdownConverter CreateSut()
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(DateTime.Today.AddDays(_random.Next(0, 365) * -1));

            _claimResolverMock.Setup(m => m.GetName())
                .Returns(_fixture.Create<string>());
            _claimResolverMock.Setup(m => m.GetMailAddress())
                .Returns($"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local");

            return new BusinessLogic.Accounting.Logic.AnnualAccountingStatementToMarkdownConverter(_statusDateProviderMock.Object, _claimResolverMock.Object);
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