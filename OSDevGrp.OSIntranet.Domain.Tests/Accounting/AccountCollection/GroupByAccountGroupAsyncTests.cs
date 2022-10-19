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
        public async Task GroupByAccountGroupAsync_WhenCalled_AssertCalculateAsyncWasCalledOnEachUniqueAccountGroupInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            Mock<IAccountGroup>[] accountGroupMockCollection =
            {
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock()
            };
            sut.Add(CreateAccountCollection(accountGroupMockCollection.Select(accountGroupMock => accountGroupMock.Object).ToArray()));

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 365) * -1);
            await sut.CalculateAsync(statusDate);

            await sut.GroupByAccountGroupAsync();

            foreach (Mock<IAccountGroup> accountGroupMock in accountGroupMockCollection)
            {
                accountGroupMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date), It.Is<IAccountCollection>(value => value != null)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsNotNull()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(CreateAccountCollection());

            IEnumerable<IAccountGroupStatus> result = await sut.GroupByAccountGroupAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsNotEmpty()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(CreateAccountCollection());

            IEnumerable<IAccountGroupStatus> result = await sut.GroupByAccountGroupAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GroupByAccountGroupAsync_WhenCalled_ReturnsAccountGroupStatusCollectionWhichContainsCalculatedAccountGroupStatusFromEachUniqueAccountGroupInAccountCollection()
        {
            IAccountCollection sut = CreateSut();

            int accountGroupNumber1 = _fixture.Create<int>();
            int accountGroupNumber2 = _fixture.Create<int>();
            int accountGroupNumber3 = _fixture.Create<int>();
            IAccountGroupStatus[] calculatedAccountGroupStatusCollection =
            {
                _fixture.BuildAccountGroupStatusMock(accountGroupNumber1).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupNumber2).Object,
                _fixture.BuildAccountGroupStatusMock(accountGroupNumber3).Object
            };

            IAccountGroup[] accountGroupCollection =
            {
                _fixture.BuildAccountGroupMock(accountGroupNumber1, calculatedAccountGroupStatus: calculatedAccountGroupStatusCollection[0]).Object,
                _fixture.BuildAccountGroupMock(accountGroupNumber2, calculatedAccountGroupStatus: calculatedAccountGroupStatusCollection[1]).Object,
                _fixture.BuildAccountGroupMock(accountGroupNumber3, calculatedAccountGroupStatus: calculatedAccountGroupStatusCollection[2]).Object
            };
            sut.Add(CreateAccountCollection(accountGroupCollection));

            IEnumerable<IAccountGroupStatus> result = await sut.GroupByAccountGroupAsync();

            Assert.That(calculatedAccountGroupStatusCollection.All(calculatedAccountGroupStatus => result.Contains(calculatedAccountGroupStatus)), Is.True);
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
            accountGroupCollection ??= new[]
            {
                _fixture.BuildAccountGroupMock().Object,
                _fixture.BuildAccountGroupMock().Object,
                _fixture.BuildAccountGroupMock().Object
            };

            return accountGroupCollection.SelectMany(accountGroup =>
                {
                    int numberOfAccounts = _random.Next(5, 10);

                    List<Mock<IAccount>> accountMockCollection = new List<Mock<IAccount>>(numberOfAccounts);
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