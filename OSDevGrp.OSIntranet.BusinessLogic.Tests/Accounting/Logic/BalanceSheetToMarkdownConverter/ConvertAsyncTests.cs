using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.BalanceSheetToMarkdownConverter
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
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountCollectionWasCalledOnAccounting()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            await sut.ConvertAsync(accountingMock.Object);

            accountingMock.Verify(m => m.AccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGroupByAccountGroupAsyncWasCalledOnAccountCollectionFromAccounting()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollectionMock.Object).Object;
            await sut.ConvertAsync(accounting);

            accountCollectionMock.Verify(m => m.GroupByAccountGroupAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountGroupTypeWasCalledTwiceOnEachAccountGroupStatusFromGroupByAccountGroupAsync()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccountGroupStatus>[] accountGroupStatusMockCollection =
            {
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities)
            };
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: accountGroupStatusMockCollection.Select(accountGroupStatusMock => accountGroupStatusMock.Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccountGroupStatus> accountGroupStatusMock in accountGroupStatusMockCollection)
            {
                accountGroupStatusMock.Verify(m => m.AccountGroupType, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertNameWasCalledOnEachAccountGroupStatusFromGroupByAccountGroupAsync()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccountGroupStatus>[] accountGroupStatusMockCollection =
            {
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities)
            };
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: accountGroupStatusMockCollection.Select(accountGroupStatusMock => accountGroupStatusMock.Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccountGroupStatus> accountGroupStatusMock in accountGroupStatusMockCollection)
            {
                accountGroupStatusMock.Verify(m => m.Name, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesAtStatusDateWasCalledTwiceOnEachAccountGroupStatusFromGroupByAccountGroupAsync()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccountGroupStatus>[] accountGroupStatusMockCollection =
            {
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities)
            };
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: accountGroupStatusMockCollection.Select(accountGroupStatusMock => accountGroupStatusMock.Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccountGroupStatus> accountGroupStatusMock in accountGroupStatusMockCollection)
            {
                accountGroupStatusMock.Verify(m => m.ValuesAtStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAssetsWasCalledTwiceOnValuesAtStatusDateForEachAccountGroupStatusWhereAccountTypeIsAssertsFromGroupByAccountGroupAsync()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccountCollectionValues>[] accountCollectionValuesMockCollection =
            {
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock()
            };
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: accountCollectionValuesMockCollection.Select(accountCollectionValuesMock => _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets, valuesAtStatusDate: accountCollectionValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccountCollectionValues> accountCollectionValuesMock in accountCollectionValuesMockCollection)
            {
                accountCollectionValuesMock.Verify(m => m.Assets, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertLiabilitiesWasCalledTwiceOnValuesAtStatusDateForEachAccountGroupStatusWhereAccountTypeIsLiabilitiesFromGroupByAccountGroupAsync()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccountCollectionValues>[] accountCollectionValuesMockCollection =
            {
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock(),
                _fixture.BuildAccountCollectionValuesMock()
            };
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: accountCollectionValuesMockCollection.Select(accountCollectionValuesMock => _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities, valuesAtStatusDate: accountCollectionValuesMock.Object).Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccountCollectionValues> accountCollectionValuesMock in accountCollectionValuesMockCollection)
            {
                accountCollectionValuesMock.Verify(m => m.Liabilities, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountCollectionWasCalledOnEachAccountGroupStatusFromGroupByAccountGroupAsync()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccountGroupStatus>[] accountGroupStatusMockCollection =
            {
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities),
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities)
            };
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: accountGroupStatusMockCollection.Select(accountGroupStatusMock => accountGroupStatusMock.Object).ToArray()).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccountGroupStatus> accountGroupStatusMock in accountGroupStatusMockCollection)
            {
                accountGroupStatusMock.Verify(m => m.AccountCollection, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(AccountGroupType.Assets)]
        [TestCase(AccountGroupType.Liabilities)]
        public async Task ConvertAsync_WhenCalled_AssertAccountNumberWasCalledOnEachAccountInAccountCollectionForEachAccountGroupStatusFromGroupByAccountGroupAsync(AccountGroupType accountGroupType)
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccount>[] accountMockCollection =
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(accountGroupType: accountGroupType, accountCollection: _fixture.BuildAccountCollectionMock(accountCollection: accountMockCollection.Select(accountMock => accountMock.Object).ToArray()).Object).Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: new[] { accountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.AccountNumber, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(AccountGroupType.Assets)]
        [TestCase(AccountGroupType.Liabilities)]
        public async Task ConvertAsync_WhenCalled_AssertAccountNameWasCalledOnEachAccountInAccountCollectionForEachAccountGroupStatusFromGroupByAccountGroupAsync(AccountGroupType accountGroupType)
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccount>[] accountMockCollection =
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(accountGroupType: accountGroupType, accountCollection: _fixture.BuildAccountCollectionMock(accountCollection: accountMockCollection.Select(accountMock => accountMock.Object).ToArray()).Object).Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: new[] { accountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.AccountName, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(AccountGroupType.Assets)]
        [TestCase(AccountGroupType.Liabilities)]
        public async Task ConvertAsync_WhenCalled_AssertValuesAtStatusDateWasCalledOnEachAccountInAccountCollectionForEachAccountGroupStatusFromGroupByAccountGroupAsync(AccountGroupType accountGroupType)
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<IAccount>[] accountMockCollection =
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(accountGroupType: accountGroupType, accountCollection: _fixture.BuildAccountCollectionMock(accountCollection: accountMockCollection.Select(accountMock => accountMock.Object).ToArray()).Object).Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: new[] { accountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(AccountGroupType.Assets)]
        [TestCase(AccountGroupType.Liabilities)]
        public async Task ConvertAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtStatusDateForEachAccountInAccountCollectionForEachAccountGroupStatusFromGroupByAccountGroupAsync(AccountGroupType accountGroupType)
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            Mock<ICreditInfoValues>[] creditInfoValuesMockCollection =
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(accountGroupType: accountGroupType, accountCollection: _fixture.BuildAccountCollectionMock(accountCollection: creditInfoValuesMockCollection.Select(creditInfoValuesMock => _fixture.BuildAccountMock(valuesAtStatusDate: creditInfoValuesMock.Object).Object).ToArray()).Object).Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: new[] { accountGroupStatus }).Object;
            IAccounting accounting = _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
            await sut.ConvertAsync(accounting);

            foreach (Mock<ICreditInfoValues> creditInfoValuesMock in creditInfoValuesMockCollection)
            {
                creditInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotNull()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotEmpty()
        {
            IBalanceSheetToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(CreateAccountingWithAccountGroupStatusCollection());

            Assert.That(result, Is.Not.Empty);
        }

        private IBalanceSheetToMarkdownConverter CreateSut(DateTime? statusDate = null)
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(statusDate?.Date ?? DateTime.Today.AddDays(_random.Next(0, 365) * -1));

            _claimResolverMock.Setup(m => m.GetName())
                .Returns(_fixture.Create<string>());
            _claimResolverMock.Setup(m => m.GetMailAddress())
                .Returns($"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local");

            return new BusinessLogic.Accounting.Logic.BalanceSheetToMarkdownConverter(_statusDateProviderMock.Object, _claimResolverMock.Object);
        }

        private IAccounting CreateAccountingWithAccountGroupStatusCollection()
        {
            IAccountGroupStatus[] accountGroupStatusCollection =
            {
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: accountGroupStatusCollection).Object;
            return _fixture.BuildAccountingMock(accountCollection: accountCollection).Object;
        }

    }
}