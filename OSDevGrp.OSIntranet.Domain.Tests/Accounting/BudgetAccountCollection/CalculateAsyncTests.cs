using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountCollection
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
        public async Task CalculateAsync_WhenCalled_AssertValuesForMonthOfStatusDateWasCalledOnEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetAccount>> budgetAccountMockCollection = new List<Mock<IBudgetAccount>>
            {
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock()
            };
            sut.Add(budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.ValuesForMonthOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBudgetWasCalledOnValuesForMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostedWasCalledOnValuesForMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesForLastMonthOfStatusDateWasCalledOnEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetAccount>> budgetAccountMockCollection = new List<Mock<IBudgetAccount>>
            {
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock()
            };
            sut.Add(budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.ValuesForLastMonthOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBudgetWasCalledOnValuesForLastMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostedWasCalledOnValuesForLastMonthOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastMonthOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesForYearToDateOfStatusDateWasCalledOnEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetAccount>> budgetAccountMockCollection = new List<Mock<IBudgetAccount>>
            {
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock()
            };
            sut.Add(budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.ValuesForYearToDateOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBudgetWasCalledOnValuesForYearToDateOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForYearToDateOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostedWasCalledOnValuesForYearToDateOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForYearToDateOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertValuesForLastYearOfStatusDateWasCalledOnEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetAccount>> budgetAccountMockCollection = new List<Mock<IBudgetAccount>>
            {
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock(),
                _fixture.BuildBudgetAccountMock()
            };
            sut.Add(budgetAccountMockCollection.Select(budgetAccountMock => budgetAccountMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetAccount> budgetAccountMock in budgetAccountMockCollection)
            {
                budgetAccountMock.Verify(m => m.ValuesForLastYearOfStatusDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBudgetWasCalledOnValuesForLastYearOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastYearOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Budget, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostedWasCalledOnValuesForLastYearOfStatusDateForEachBudgetAccountInBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetInfoValuesMockCollection = new List<Mock<IBudgetInfoValues>>
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            sut.Add(budgetInfoValuesMockCollection.Select(budgetInfoValuesMock => _fixture.BuildBudgetAccountMock(valuesForLastYearOfStatusDate: budgetInfoValuesMock.Object).Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfoValues> budgetInfoValuesMock in budgetInfoValuesMockCollection)
            {
                budgetInfoValuesMock.Verify(m => m.Posted, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollection()
        {
            IBudgetAccountCollection sut = CreateSut();

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereValuesForMonthOfStatusDateIsNotNull()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereBudgetInValuesForMonthOfStatusDateIsEqualToSumOfBudgetFromValuesForMonthOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate.Budget, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForMonthOfStatusDate.Budget)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWherePostedInValuesForMonthOfStatusDateIsEqualToSumOfPostedFromValuesForMonthOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate.Posted, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForMonthOfStatusDate.Posted)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereValuesForLastMonthOfStatusDateIsNotNull()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereBudgetInValuesForLastMonthOfStatusDateIsEqualToSumOfBudgetFromValuesForLastMonthOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate.Budget, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForLastMonthOfStatusDate.Budget)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWherePostedInValuesForLastMonthOfStatusDateIsEqualToSumOfPostedFromValuesForLastMonthOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate.Posted, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForLastMonthOfStatusDate.Posted)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereValuesForYearToDateOfStatusDateIsNotNull()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereBudgetInValuesForYearToDateOfStatusDateIsEqualToSumOfBudgetFromValuesForYearToDateOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate.Budget, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForYearToDateOfStatusDate.Budget)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWherePostedInValuesForYearToDateOfStatusDateIsEqualToSumOfPostedFromValuesForYearToDateOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate.Posted, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForYearToDateOfStatusDate.Posted)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereValuesForValuesForLastYearOfStatusDateIsNotNull()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWhereBudgetInValuesForLastYearOfStatusDateIsEqualToSumOfBudgetFromValuesForLastYearOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate.Budget, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForLastYearOfStatusDate.Budget)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountCollectionWherePostedInValuesForLastYearOfStatusDateIsEqualToSumOfPostedFromValuesForLastYearOfStatusDateOnBudgetAccounts()
        {
            IBudgetAccountCollection sut = CreateSut();

            IEnumerable<IBudgetAccount> budgetAccountCollection = new List<IBudgetAccount>
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            sut.Add(budgetAccountCollection);

            IBudgetAccountCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate.Posted, Is.EqualTo(budgetAccountCollection.Sum(budgetAccount => budgetAccount.ValuesForLastYearOfStatusDate.Posted)));
        }

        private IBudgetAccountCollection CreateSut()
        {
            return new Domain.Accounting.BudgetAccountCollection();
        }
    }
}