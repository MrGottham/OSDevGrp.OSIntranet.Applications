using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountCollection
{
    [TestFixture]
    public class CalculateAsyncTests
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
        public async Task CalculateAsync_WhenCalled_AssertAccountGroupTypeWasCalledOnEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<Mock<IAccount>> accountMockCollection = new List<Mock<IAccount>>
            {
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities)
            };
            sut.Add(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.AccountGroupType, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesAtStatusDateWasCalledOnEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<Mock<IAccount>> accountMockCollection = new List<Mock<IAccount>>
            {
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities)
            };
            sut.Add(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtStatusDateForEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<Mock<ICreditInfoValues>> creditInfoValuesMockCollection = new List<Mock<ICreditInfoValues>>
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            sut.Add(creditInfoValuesMockCollection.Select(creditInfoValuesMock => _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities, valuesAtStatusDate: creditInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfoValues> creditInfoValuesMock in creditInfoValuesMockCollection)
            {
                creditInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<Mock<IAccount>> accountMockCollection = new List<Mock<IAccount>>
            {
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities)
            };
            sut.Add(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtEndOfLastMonthFromStatusDateForEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<Mock<ICreditInfoValues>> creditInfoValuesMockCollection = new List<Mock<ICreditInfoValues>>
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            sut.Add(creditInfoValuesMockCollection.Select(creditInfoValuesMock => _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities, valuesAtEndOfLastMonthFromStatusDate: creditInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfoValues> creditInfoValuesMock in creditInfoValuesMockCollection)
            {
                creditInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<Mock<IAccount>> accountMockCollection = new List<Mock<IAccount>>
            {
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities),
                _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities)
            };
            sut.Add(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtEndOfLastYearFromStatusDateForEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<Mock<ICreditInfoValues>> creditInfoValuesMockCollection = new List<Mock<ICreditInfoValues>>
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            sut.Add(creditInfoValuesMockCollection.Select(creditInfoValuesMock => _fixture.BuildAccountMock(accountGroupType: _random.Next(100) > 50 ? AccountGroupType.Assets : AccountGroupType.Liabilities, valuesAtEndOfLastYearFromStatusDate: creditInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfoValues> creditInfoValuesMock in creditInfoValuesMockCollection)
            {
                creditInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnAssetAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Assets, Is.EqualTo(accountCollection.Sum(account => account.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtStatusDateIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Liabilities, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnAssetAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Assets, Is.EqualTo(accountCollection.Sum(account => account.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtEndOfLastMonthFromStatusDateIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Liabilities, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastYearFromStatusDateOnAssetAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Assets, Is.EqualTo(accountCollection.Sum(account => account.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsAssetAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtEndOfLastYearFromStatusDateIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Liabilities, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtStatusDateIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Assets, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnLiabilityAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Liabilities, Is.EqualTo(accountCollection.Sum(account => account.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtEndOfLastMonthFromStatusDateIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Assets, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnLiabilityAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Liabilities, Is.EqualTo(accountCollection.Sum(account => account.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtEndOfLastYearFromStatusDateIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Assets, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionOnlyContainsLiabilityAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtValuesAtEndOfLastYearFromStatusDateOnLiabilityAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> accountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(accountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Liabilities, Is.EqualTo(accountCollection.Sum(account => account.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnAssetAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Assets, Is.EqualTo(assetAccountCollection.Sum(account => account.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnLiabilityAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Liabilities, Is.EqualTo(liabilityAccountCollection.Sum(account => account.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnAssetAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Assets, Is.EqualTo(assetAccountCollection.Sum(account => account.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnLiabilityAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Liabilities, Is.EqualTo(liabilityAccountCollection.Sum(account => account.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereAssetsInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtValuesAtEndOfLastYearFromStatusDateOnAssetAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Assets, Is.EqualTo(assetAccountCollection.Sum(account => account.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenAccountCollectionContainsBothAssetAndLiabilityAccounts_ReturnsAccountCollectionWhereLiabilitiesInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtValuesAtEndOfLastYearFromStatusDateOnLiabilityAccounts()
        {
            IAccountCollection sut = CreateSut();

            IEnumerable<IAccount> assetAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets).Object
            };
            sut.Add(assetAccountCollection);

            IEnumerable<IAccount> liabilityAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object,
                _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities).Object
            };
            sut.Add(liabilityAccountCollection);

            IAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Liabilities, Is.EqualTo(liabilityAccountCollection.Sum(account => account.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        private IAccountCollection CreateSut()
        {
            return new Domain.Accounting.AccountCollection();
        }
    }
}