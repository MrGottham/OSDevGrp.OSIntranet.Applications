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
    public class FindCreditorsAsyncTests
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
        public async Task FindCreditorsAsync_WhenCalled_AssertContactAccountTypeWasCalledOnEachContactAccountInContactAccountCollection()
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

            await sut.FindCreditorsAsync();

            foreach (Mock<IContactAccount> contactAccountMock in contactAccountMockCollection)
            {
                contactAccountMock.Verify(m => m.ContactAccountType, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenCalled_AssertAccountNameWasCalledOnEachCreditorContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<Mock<IContactAccount>> creditorContactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor)
            };
            sut.Add(creditorContactAccountMockCollection.Select(creditorContactAccountMock => creditorContactAccountMock.Object).ToArray());

            IEnumerable<IContactAccount> nonCreditorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(nonCreditorContactAccountCollection);

            await sut.FindCreditorsAsync();

            foreach (Mock<IContactAccount> creditorContactAccountMock in creditorContactAccountMockCollection)
            {
                creditorContactAccountMock.Verify(m => m.AccountName, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenCalled_AssertAccountNameWasNotCalledOnAnyNoneCreditorContactAccountInContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> creditorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorContactAccountCollection);

            IEnumerable<Mock<IContactAccount>> nonCreditorContactAccountMockCollection = new List<Mock<IContactAccount>>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor),
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor)
            };
            sut.Add(nonCreditorContactAccountMockCollection.Select(nonCreditorContactAccountMock => nonCreditorContactAccountMock.Object).ToArray());

            await sut.FindCreditorsAsync();

            foreach (Mock<IContactAccount> nonCreditorContactAccountMock in nonCreditorContactAccountMockCollection)
            {
                nonCreditorContactAccountMock.Verify(m => m.AccountName, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenContactAccountCollectionIsEmpty_ReturnsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            IContactAccountCollection result = await sut.FindCreditorsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenContactAccountCollectionIsEmpty_ReturnsCalculatedAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IContactAccountCollection result = await (await sut.CalculateAsync(statusDate)).FindCreditorsAsync();

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenContactAccountCollectionContainsContactAccounts_ReturnsNotNull()
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

            IContactAccountCollection result = await sut.FindCreditorsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenContactAccountCollectionContainsContactAccounts_ReturnsCalculatedAccountCollection()
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
            IContactAccountCollection result = await (await sut.CalculateAsync(statusDate)).FindCreditorsAsync();

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenContactAccountCollectionContainsOnlyNonCreditorContactAccounts_ReturnsEmptyAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> nonCreditorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(nonCreditorContactAccountCollection);

            IContactAccountCollection result = await sut.FindCreditorsAsync();

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task FindCreditorsAsync_WhenContactAccountCollectionContainsCreditorContactAccounts_ReturnsAccountCollectionWithCreditorContactAccountsFromContactAccountCollection()
        {
            IContactAccountCollection sut = CreateSut();

            IEnumerable<IContactAccount> creditorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Creditor).Object
            };
            sut.Add(creditorContactAccountCollection);

            IEnumerable<IContactAccount> nonCreditorContactAccountCollection = new List<IContactAccount>
            {
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.None).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object,
                _fixture.BuildContactAccountMock(contactAccountType: ContactAccountType.Debtor).Object
            };
            sut.Add(nonCreditorContactAccountCollection);

            IContactAccountCollection result = await sut.FindCreditorsAsync();

            Assert.That(result.Count(), Is.EqualTo(creditorContactAccountCollection.Count()));
            Assert.That(result.All(contactAccount => creditorContactAccountCollection.Contains(contactAccount)), Is.True);
        }

        private IContactAccountCollection CreateSut()
        {
            return new Domain.Accounting.ContactAccountCollection();
        }
    }
}