using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.CreditInfoCollection
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
        public async Task CalculateAsync_WhenCalled_AssertIsMonthOfStatusDateWasCalledOnEachCreditInfoInCreditInfoCollection()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<ICreditInfo>> creditInfoMockCollection = new List<Mock<ICreditInfo>>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset, true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2))
            };
            sut.Add(creditInfoMockCollection.Select(creditInfoMock => creditInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                creditInfoMock.Verify(m => m.IsMonthOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertIsLastMonthOfStatusDateWasCalledOnEachCreditInfoInCreditInfoCollection()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<ICreditInfo>> creditInfoMockCollection = new List<Mock<ICreditInfo>>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset, isLastMonthOfStatusDate: true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2))
            };
            sut.Add(creditInfoMockCollection.Select(creditInfoMock => creditInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                creditInfoMock.Verify(m => m.IsLastMonthOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertIsLastYearOfStatusDateWasCalledOnEachCreditInfoInCreditInfoCollection()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<ICreditInfo>> creditInfoMockCollection = new List<Mock<ICreditInfo>>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset, isLastYearOfStatusDate: true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2))
            };
            sut.Add(creditInfoMockCollection.Select(creditInfoMock => creditInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                creditInfoMock.Verify(m => m.IsLastYearOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertYearWasCalledOnEachCreditInfoFromLastYearOfStatusDateInCreditInfoCollection()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<ICreditInfo>> creditInfoMockCollection = new List<Mock<ICreditInfo>>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset, isLastYearOfStatusDate: true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2))
            };
            sut.Add(creditInfoMockCollection.Select(creditInfoMock => creditInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                if (creditInfoMock.Object.IsLastYearOfStatusDate)
                {
                    creditInfoMock.Verify(m => m.Year, Times.Once);
                    continue;
                }

                creditInfoMock.Verify(m => m.Year, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertMonthWasCalledOnEachCreditInfoInCreditInfoCollection()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<ICreditInfo>> creditInfoMockCollection = new List<Mock<ICreditInfo>>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset, isLastYearOfStatusDate: true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true),
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2))
            };
            sut.Add(creditInfoMockCollection.Select(creditInfoMock => creditInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                if (creditInfoMock.Object.IsLastYearOfStatusDate)
                {
                    creditInfoMock.Verify(m => m.Month, Times.Once);
                    continue;
                }

                creditInfoMock.Verify(m => m.Month, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsCreditInfoCollection()
        {
            ICreditInfoCollection sut = CreateSut();

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForStatusDate_ReturnsCreditInfoCollectionWhereValuesAtStatusDateIsNotNull()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForStatusDate_ReturnsCreditInfoCollectionWhereCreditInValuesAtStatusDateIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForStatusDate_ReturnsCreditInfoCollectionWhereBalanceInValuesAtStatusDateIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForStatusDate_ReturnsCreditInfoCollectionWhereValuesAtStatusDateIsNotNull()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForStatusDate_ReturnsCreditInfoCollectionWhereCreditInValuesAtStatusDateIsEqualToCreditInCreditInfoForStatusDate()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Credit, Is.EqualTo(creditInfoForStatusDate.Credit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForStatusDate_ReturnsCreditInfoCollectionWhereBalanceInValuesAtStatusDateIsEqualToBalanceInCreditInfoForStatusDate()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Balance, Is.EqualTo(creditInfoForStatusDate.Balance));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForLastMonthFromStatusDate_ReturnsCreditInfoCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForLastMonthFromStatusDate_ReturnsCreditInfoCollectionWhereCreditInValuesAtEndOfLastMonthFromStatusDateIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForLastMonthFromStatusDate_ReturnsCreditInfoCollectionWhereBalanceInValuesAtEndOfLastMonthFromStatusDateIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForLastMonthFromStatusDate_ReturnsCreditInfoCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForLastMonthFromStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForLastMonthFromStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForLastMonthFromStatusDate_ReturnsCreditInfoCollectionWhereCreditInValuesAtEndOfLastMonthFromStatusDateIsEqualToCreditInCreditInfoForLastMonthFromStatusDate()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForLastMonthFromStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForLastMonthFromStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Credit, Is.EqualTo(creditInfoForLastMonthFromStatusDate.Credit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForLastMonthFromStatusDate_ReturnsCreditInfoCollectionWhereBalanceInValuesAtEndOfLastMonthFromStatusDateIsEqualToBalanceInCreditInfoForLastMonthFromStatusDate()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForLastMonthFromStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForLastMonthFromStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Balance, Is.EqualTo(creditInfoForLastMonthFromStatusDate.Balance));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForEndOfLastYearFromStatusDate_ReturnsCreditInfoCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForEndOfLastYearFromStatusDate_ReturnsCreditInfoCollectionWhereCreditInValuesAtEndOfLastYearFromStatusDateIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionDoesNotContainCreditInfoForEndOfLastYearFromStatusDate_ReturnsCreditInfoCollectionWhereBalanceInValuesAtEndOfLastYearFromStatusDateIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock(creditInfoOffset).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForEndOfLastYearFromStatusDate_ReturnsCreditInfoCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForEndOfLastYearFromStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, isLastYearOfStatusDate: true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForEndOfLastYearFromStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2), isLastYearOfStatusDate: true).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForEndOfLastYearFromStatusDate_ReturnsCreditInfoCollectionWhereCreditInValuesAtEndOfLastYearFromStatusDateIsEqualToCreditInCreditInfoForEndOfLastYearFromStatusDate()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForEndOfLastYearFromStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, isLastYearOfStatusDate: true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForEndOfLastYearFromStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2), isLastYearOfStatusDate: true).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Credit, Is.EqualTo(creditInfoForEndOfLastYearFromStatusDate.Credit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCreditInfoCollectionContainsCreditInfoForEndOfLastYearFromStatusDate_ReturnsCreditInfoCollectionWhereBalanceInValuesAtEndOfLastYearFromStatusDateIsEqualToBalanceInCreditInfoForEndOfLastYearFromStatusDate()
        {
            ICreditInfoCollection sut = CreateSut();

            DateTime creditInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            ICreditInfo creditInfoForEndOfLastYearFromStatusDate = _fixture.BuildCreditInfoMock(creditInfoOffset, isLastYearOfStatusDate: true).Object;
            IEnumerable<ICreditInfo> creditInfoCollection = new List<ICreditInfo>
            {
                creditInfoForEndOfLastYearFromStatusDate,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildCreditInfoMock(creditInfoOffset.AddMonths(-2), isLastYearOfStatusDate: true).Object
            };
            sut.Add(creditInfoCollection);

            ICreditInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Balance, Is.EqualTo(creditInfoForEndOfLastYearFromStatusDate.Balance));
        }

        private ICreditInfoCollection CreateSut()
        {
            return new Domain.Accounting.CreditInfoCollection();
        }
    }
}