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
    public class FindDebtorsAsyncTests
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
        public async Task FindDebtorsAsync_WhenCalled_AssertContactAccountTypeWasCalledOnEachContactAccountInContactAccountCollection()
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

            await sut.FindDebtorsAsync();

            foreach (Mock<IContactAccount> contactAccountMock in contactAccountMockCollection)
            {
                contactAccountMock.Verify(m => m.ContactAccountType, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenCalled_AssertAccountNameWasCalledOnEachDebtorContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactAccount>> debtorContactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor)
            };
            sut.Add(debtorContactAccountMockCollection.Select(debtorContactAccountMock => debtorContactAccountMock.Object).ToArray());

            IEnumerable<IContactAccount> nonDebtorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(nonDebtorContactAccountCollection);

            await sut.FindDebtorsAsync();

            foreach (Mock<IContactAccount> debtorContactAccountMock in debtorContactAccountMockCollection)
            {
                debtorContactAccountMock.Verify(m => m.AccountName, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenCalled_AssertAccountNameWasNotCalledOnAnyNoneDebtorContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorContactAccountCollection);

            IEnumerable<Mock<IContactAccount>> nonDebtorContactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor)
            };
            sut.Add(nonDebtorContactAccountMockCollection.Select(nonDebtorContactAccountMock => nonDebtorContactAccountMock.Object).ToArray());

            await sut.FindDebtorsAsync();

            foreach (Mock<IContactAccount> nonDebtorContactAccountMock in nonDebtorContactAccountMockCollection)
            {
                nonDebtorContactAccountMock.Verify(m => m.AccountName, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenContactAccountCollectionIsEmpty_ReturnsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IContactAccountCollection result = await sut.FindDebtorsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenContactAccountCollectionIsEmpty_ReturnsCalculatedAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IContactAccountCollection result = await (await sut.CalculateAsync(statusDate)).FindDebtorsAsync();

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenContactAccountCollectionContainsContactAccounts_ReturnsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            IContactAccountCollection result = await sut.FindDebtorsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenContactAccountCollectionContainsContactAccounts_ReturnsCalculatedAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> contactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: _random.Next(100) > 50 ? ContactAccountType.Debtor : ContactAccountType.Creditor).Object
            };
            sut.Add(contactAccountCollection);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IContactAccountCollection result = await (await sut.CalculateAsync(statusDate)).FindDebtorsAsync();

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenContactAccountCollectionContainsOnlyNonDebtorContactAccounts_ReturnsEmptyAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> nonDebtorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(nonDebtorContactAccountCollection);

            IContactAccountCollection result = await sut.FindDebtorsAsync();

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindDebtorsAsync_WhenContactAccountCollectionContainsDebtorContactAccounts_ReturnsAccountCollectionWithDebtorContactAccountsFromContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> debtorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(debtorContactAccountCollection);

            IEnumerable<IContactAccount> nonDebtorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(nonDebtorContactAccountCollection);

            IContactAccountCollection result = await sut.FindDebtorsAsync();

            Assert.That(result.Count(), Is.EqualTo(debtorContactAccountCollection.Count()));
            Assert.That(result.All(contactAccount => debtorContactAccountCollection.Contains(contactAccount)), Is.True);
        }

        private IContactAccountCollection CreateSut()
        {
            return new Domain.Accounting.ContactAccountCollection();
        }
    }
}