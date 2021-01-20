using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetInfoCollection
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
        public async Task CalculateAsync_WhenCalled_AssertIsMonthOfStatusDateWasCalledOnEachBudgetInfoInBudgetInfoCollection()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IBudgetInfo>> budgetInfoMockCollection = new List<Mock<IBudgetInfo>>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, true),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2))
            };
            sut.Add(budgetInfoMockCollection.Select(budgetInfoMock => budgetInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfo> budgetInfoMock in budgetInfoMockCollection)
            {
                budgetInfoMock.Verify(m => m.IsMonthOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertIsLastMonthOfStatusDateWasCalledOnEachBudgetInfoInBudgetInfoCollection()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IBudgetInfo>> budgetInfoMockCollection = new List<Mock<IBudgetInfo>>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastMonthOfStatusDate: true),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2))
            };
            sut.Add(budgetInfoMockCollection.Select(budgetInfoMock => budgetInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfo> budgetInfoMock in budgetInfoMockCollection)
            {
                budgetInfoMock.Verify(m => m.IsLastMonthOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertIsYearToDateOfStatusDateWasCalledOnEachBudgetInfoInBudgetInfoCollection()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IBudgetInfo>> budgetInfoMockCollection = new List<Mock<IBudgetInfo>>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isYearToDateOfStatusDate: true),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isYearToDateOfStatusDate: true),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2))
            };
            sut.Add(budgetInfoMockCollection.Select(budgetInfoMock => budgetInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfo> budgetInfoMock in budgetInfoMockCollection)
            {
                budgetInfoMock.Verify(m => m.IsYearToDateOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertIsLastYearOfStatusDateWasCalledOnEachBudgetInfoInBudgetInfoCollection()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IBudgetInfo>> budgetInfoMockCollection = new List<Mock<IBudgetInfo>>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastYearOfStatusDate: true),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true),
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2))
            };
            sut.Add(budgetInfoMockCollection.Select(budgetInfoMock => budgetInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IBudgetInfo> budgetInfoMock in budgetInfoMockCollection)
            {
                budgetInfoMock.Verify(m => m.IsLastYearOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetInfoCollection()
        {
            IBudgetInfoCollection sut = CreateSut();

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForMonthOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForMonthOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForMonthOfStatusDate_ReturnsBudgetInfoCollectionWherePostedInValuesForMonthOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate.Posted, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainsBudgetInfoForMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForMonthOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IBudgetInfo budgetInfoForMonthOfStatusDate = _fixture.BuildBudgetInfoMock(budgetInfoOffset, true).Object;
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                budgetInfoForMonthOfStatusDate,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainsBudgetInfoForMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForMonthOfStatusDateIsEqualToBudgetInBudgetInfoForMonthOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IBudgetInfo budgetInfoForMonthOfStatusDate = _fixture.BuildBudgetInfoMock(budgetInfoOffset, true).Object;
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                budgetInfoForMonthOfStatusDate,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate.Budget, Is.EqualTo(budgetInfoForMonthOfStatusDate.Budget));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainsBudgetInfoForMonthOfStatusDate_ReturnsBudgetInfoCollectionWherePostedInValuesForMonthOfStatusDateIsEqualToPostedInBudgetInfoForMonthOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IBudgetInfo budgetInfoForMonthOfStatusDate = _fixture.BuildBudgetInfoMock(budgetInfoOffset, true).Object;
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                budgetInfoForMonthOfStatusDate,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForMonthOfStatusDate.Posted, Is.EqualTo(budgetInfoForMonthOfStatusDate.Posted));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForLastMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForLastMonthOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForLastMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForLastMonthOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForLastMonthOfStatusDate_ReturnsBudgetInfoCollectionWherePostedInValuesForLastMonthOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate.Posted, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForLastMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForLastMonthOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IBudgetInfo budgetInfoForLastMonthOfStatusDate = _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                budgetInfoForLastMonthOfStatusDate,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForLastMonthOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForLastMonthOfStatusDateIsEqualToBudgetInBudgetInfoForLastMonthOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IBudgetInfo budgetInfoForLastMonthOfStatusDate = _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                budgetInfoForLastMonthOfStatusDate,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate.Budget, Is.EqualTo(budgetInfoForLastMonthOfStatusDate.Budget));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForLastMonthOfStatusDate_ReturnsBudgetInfoCollectionWherePostedInValuesForLastMonthOfStatusDateIsEqualToPostedInBudgetInfoForLastMonthOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IBudgetInfo budgetInfoForLastMonthOfStatusDate = _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                budgetInfoForLastMonthOfStatusDate,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastMonthOfStatusDate.Posted, Is.EqualTo(budgetInfoForLastMonthOfStatusDate.Posted));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForYearToDateOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForYearToDateOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForYearToDateOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForYearToDateOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForYearToDateOfStatusDate_ReturnsBudgetInfoCollectionWherePostedInValuesForYearToDateOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate.Posted, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForYearToDateOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForYearToDateOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isYearToDateOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isYearToDateOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForYearToDateOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForYearToDateOfStatusDateIsEqualToSumOfBudgetFromBudgetInfoForYearToDateOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isYearToDateOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isYearToDateOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate.Budget, Is.EqualTo(budgetInfoCollection.Where(budgetInfo => budgetInfo.IsYearToDateOfStatusDate).Sum(budgetInfo => budgetInfo.Budget)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForYearToDateOfStatusDate_ReturnsBudgetInfoCollectionWherePostedInValuesForYearToDateOfStatusDateIsEqualToSumOfPostedFromBudgetInfoForYearToDateOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isYearToDateOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isYearToDateOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForYearToDateOfStatusDate.Posted, Is.EqualTo(budgetInfoCollection.Where(budgetInfo => budgetInfo.IsYearToDateOfStatusDate).Sum(budgetInfo => budgetInfo.Posted)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForLastYearOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForLastYearOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForLastYearOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForLastYearOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionDoesNotContainBudgetInfoForLastYearOfStatusDate_ReturnsBudgetInfoCollectionWherePostInValuesForLastYearOfStatusDateIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate.Posted, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForLastYearOfStatusDate_ReturnsBudgetInfoCollectionWhereValuesForLastYearOfStatusDateIsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastYearOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForLastYearOfStatusDate_ReturnsBudgetInfoCollectionWhereBudgetInValuesForLastYearOfStatusDateIsEqualToSumOfBudgetFromBudgetInfoForLastYearOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastYearOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate.Budget, Is.EqualTo(budgetInfoCollection.Where(budgetInfo => budgetInfo.IsLastYearOfStatusDate).Sum(budgetInfo => budgetInfo.Budget)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenBudgetInfoCollectionContainBudgetInfoForLastYearOfStatusDate_ReturnsBudgetInfoCollectionWherePostInValuesForLastYearOfStatusDateIsEqualToSumOfPostedFromBudgetInfoForLastYearOfStatusDate()
        {
            IBudgetInfoCollection sut = CreateSut();

            DateTime budgetInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IBudgetInfo> budgetInfoCollection = new List<IBudgetInfo>
            {
                _fixture.BuildBudgetInfoMock(budgetInfoOffset, isLastYearOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildBudgetInfoMock(budgetInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(budgetInfoCollection);

            IBudgetInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesForLastYearOfStatusDate.Posted, Is.EqualTo(budgetInfoCollection.Where(budgetInfo => budgetInfo.IsLastYearOfStatusDate).Sum(budgetInfo => budgetInfo.Posted)));
        }

        private IBudgetInfoCollection CreateSut()
        {
            return new Domain.Accounting.BudgetInfoCollection();
        }
    }
}