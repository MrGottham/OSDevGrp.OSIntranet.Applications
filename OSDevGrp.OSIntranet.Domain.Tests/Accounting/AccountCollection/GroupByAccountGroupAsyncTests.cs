using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountCollection
{
    [TestFixture]
    public class GroupByAccountGroupAsyncTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_AssertAccountGroupWasCalledOnEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            Mock<IAccount>[] accountMockCollection = CreateAccountMockCollection();
            sut.Add(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            await sut.GroupByAccountGroupAsync();

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.AccountGroup, Times.AtLeastOnce);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_AssertNumberWasCalledOnAccountGroupForEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            Mock<IAccountGroup>[] accountGroupMockCollection =
            {
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock()
            };
            sut.Add(CreateAccountCollection(accountGroupMockCollection.Select(accountGroupMock => accountGroupMock.Object).ToArray()));

            await sut.GroupByAccountGroupAsync();

            foreach (Mock<IAccountGroup> accountGroupMock in accountGroupMockCollection)
            {
                accountGroupMock.Verify(m => m.Number, Times.AtLeastOnce);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_AssertAccountNumberWasCalledOnEachAccountInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            Mock<IAccount>[] accountMockCollection = CreateAccountMockCollection();
            sut.Add(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            await sut.GroupByAccountGroupAsync();

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.AccountNumber, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsNotNull()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(CreateAccountCollection());

            IReadOnlyDictionary<IAccountGroup, IAccountCollection> result = await sut.GroupByAccountGroupAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhichContainsEachAccountGroupFromAccountsInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IAccountGroup[] accountGroupCollection = _fixture.CreateMany<IAccountGroup>(_random.Next(2, 5)).ToArray();
            sut.Add(CreateAccountCollection(accountGroupCollection));

            IReadOnlyDictionary<IAccountGroup, IAccountCollection> result = await sut.GroupByAccountGroupAsync();

            Assert.That(accountGroupCollection.All(accountGroup => result.Keys.Single(key => key.Number == accountGroup.Number) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhichContainsAccountCollectionMatchingEachAccountGroupFromAccountsInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(CreateAccountCollection());

            IReadOnlyDictionary<IAccountGroup, IAccountCollection> result = await sut.GroupByAccountGroupAsync();

            Assert.That(result.All(item => item.Value.All(account => account.AccountGroup.Number == item.Key.Number)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhichContainsAllAccountsInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            IAccount[] accountCollection = CreateAccountCollection();
            sut.Add(accountCollection);

            IReadOnlyDictionary<IAccountGroup, IAccountCollection> result = await sut.GroupByAccountGroupAsync();

            Assert.That(accountCollection.All(account => result.SelectMany(item => item.Value).Contains(account)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsReadOnlyDictionaryWhereAllAccountCollectionsIsCalculated()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(CreateAccountCollection());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IReadOnlyDictionary<IAccountGroup, IAccountCollection> result = await (await sut.CalculateAsync(statusDate)).GroupByAccountGroupAsync();

            Assert.That(result.Select(item => item.Value.StatusDate).All(value => value == statusDate.Date), Is.True);
        }

        private IAccountCollection CreateSut()
        {
            return new Domain.Accounting.AccountCollection();
        }

        private IAccount[] CreateAccountCollection(IEnumerable<IAccountGroup> accountGroupCollection = null)
        {
            return CreateAccountMockCollection(accountGroupCollection)
                .Select(accountMock => accountMock.Object)
                .ToArray();
        }

        private Mock<IAccount>[] CreateAccountMockCollection(IEnumerable<IAccountGroup> accountGroupCollection = null)
        {
            accountGroupCollection ??= _fixture.CreateMany<IAccountGroup>(_random.Next(2, 5));

            return accountGroupCollection.SelectMany(accountGroup =>
                {
                    int numberOfAccounts = _random.Next(5, 10);

                    List<Mock<IAccount>> accountMockCollection = new List<Mock<IAccount>>();
                    while (accountMockCollection.Count < numberOfAccounts)
                    {
                        accountMockCollection.Add(_fixture.BuildAccountMock(accountGroup: accountGroup));
                    }

                    return accountMockCollection;
                })
                .ToArray();
        }
    }
}