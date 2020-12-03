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

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactAccountCollection
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
        public async Task CalculateAsync_WhenCalled_AssertContactAccountTypeWasCalledOnEachContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactAccount>> contactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor)
            };
            sut.Add(contactAccountMockCollection.Select(contactAccountMock => contactAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactAccount> contactAccountMock in contactAccountMockCollection)
            {
                contactAccountMock.Verify(m => m.ContactAccountType, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesAtStatusDateWasCalledOnEachContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactAccount>> contactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor)
            };
            sut.Add(contactAccountMockCollection.Select(contactAccountMock => contactAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactAccount> contactAccountMock in contactAccountMockCollection)
            {
                contactAccountMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtStatusDateForEachContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactInfoValues>> contactInfoValuesMockCollection = new List<Mock<IContactInfoValues>>
            {
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock()
            };
            sut.Add(contactInfoValuesMockCollection.Select(contactInfoValuesMock => _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor, valuesAtStatusDate: contactInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfoValues> contactInfoValuesMock in contactInfoValuesMockCollection)
            {
                contactInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnEachContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactAccount>> contactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor)
            };
            sut.Add(contactAccountMockCollection.Select(contactAccountMock => contactAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactAccount> contactAccountMock in contactAccountMockCollection)
            {
                contactAccountMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtEndOfLastMonthFromStatusDateForEachContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactInfoValues>> contactInfoValuesMockCollection = new List<Mock<IContactInfoValues>>
            {
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock()
            };
            sut.Add(contactInfoValuesMockCollection.Select(contactInfoValuesMock => _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor, valuesAtEndOfLastMonthFromStatusDate: contactInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfoValues> contactInfoValuesMock in contactInfoValuesMockCollection)
            {
                contactInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnEachContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactAccount>> contactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor)
            };
            sut.Add(contactAccountMockCollection.Select(contactAccountMock => contactAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactAccount> contactAccountMock in contactAccountMockCollection)
            {
                contactAccountMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtEndOfLastYearFromStatusDateForEachContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactInfoValues>> contactInfoValuesMockCollection = new List<Mock<IContactInfoValues>>
            {
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock()
            };
            sut.Add(contactInfoValuesMockCollection.Select(contactInfoValuesMock => _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor, valuesAtEndOfLastYearFromStatusDate: contactInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfoValues> contactInfoValuesMock in contactInfoValuesMockCollection)
            {
                contactInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnDebtorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Debtors, Is.EqualTo(contactAccountCollection.Sum(contactAccount => contactAccount.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtStatusDateIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Creditors, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnDebtorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Debtors, Is.EqualTo(contactAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtEndOfLastMonthFromStatusDateIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Creditors, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastYearFromStatusDateOnDebtorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Debtors, Is.EqualTo(contactAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsDebtorAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtEndOfLastYearFromStatusDateIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Creditors, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtStatusDateIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Debtors, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnCreditorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Creditors, Is.EqualTo(contactAccountCollection.Sum(contactAccount => contactAccount.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtEndOfLastMonthFromStatusDateIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Debtors, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnCreditorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Creditors, Is.EqualTo(contactAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtEndOfLastYearFromStatusDateIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Debtors, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionOnlyContainsCreditorAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastYearFromStatusDateOnCreditorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Creditors, Is.EqualTo(contactAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnDebtorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Debtors, Is.EqualTo(debtorAccountCollection.Sum(contactAccount => contactAccount.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtStatusDateIsEqualToSumOfBalanceFromValuesAtStatusDateOnCreditorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Creditors, Is.EqualTo(creditorAccountCollection.Sum(contactAccount => contactAccount.ValuesAtStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnDebtorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Debtors, Is.EqualTo(debtorAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtEndOfLastMonthFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastMonthFromStatusDateOnCreditorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Creditors, Is.EqualTo(creditorAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastMonthFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereDebtorsInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastYearFromStatusDateOnDebtorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Debtors, Is.EqualTo(debtorAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactAccountCollectionContainsBothDebtorAndCreditAccounts_ReturnsContactAccountCollectionWhereCreditorsInValuesAtEndOfLastYearFromStatusDateIsEqualToSumOfBalanceFromValuesAtEndOfLastYearFromStatusDateOnCreditorAccounts()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorAccountCollection);

            IEnumerable<IContactAccount> creditorAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorAccountCollection);

            IContactAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Creditors, Is.EqualTo(creditorAccountCollection.Sum(contactAccount => contactAccount.ValuesAtEndOfLastYearFromStatusDate.Balance)));
        }

        private IContactAccountCollection CreateSut()
        {
            return new Domain.Accounting.ContactAccountCollection();
        }
    }
}