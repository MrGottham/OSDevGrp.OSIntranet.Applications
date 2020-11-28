using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactInfoCollection
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
        public async Task CalculateAsync_WhenCalled_AssertIsMonthOfStatusDateWasCalledOnEachContactInfoInContactInfoCollection()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IContactInfo>> contactInfoMockCollection = new List<Mock<IContactInfo>>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset, true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2))
            };
            sut.Add(contactInfoMockCollection.Select(contactInfoMock => contactInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfo> contactInfoMock in contactInfoMockCollection)
            {
                contactInfoMock.Verify(m => m.IsMonthOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertIsLastMonthOfStatusDateWasCalledOnEachContactInfoInContactInfoCollection()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IContactInfo>> contactInfoMockCollection = new List<Mock<IContactInfo>>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset, isLastMonthOfStatusDate: true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2))
            };
            sut.Add(contactInfoMockCollection.Select(contactInfoMock => contactInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfo> contactInfoMock in contactInfoMockCollection)
            {
                contactInfoMock.Verify(m => m.IsLastMonthOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertIsLastYearOfStatusDateWasCalledOnEachContactInfoInContactInfoCollection()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IContactInfo>> contactInfoMockCollection = new List<Mock<IContactInfo>>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset, isLastYearOfStatusDate: true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2))
            };
            sut.Add(contactInfoMockCollection.Select(contactInfoMock => contactInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfo> contactInfoMock in contactInfoMockCollection)
            {
                contactInfoMock.Verify(m => m.IsLastYearOfStatusDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertYearWasCalledOnEachContactInfoFromLastYearOfStatusDateInContactInfoCollection()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IContactInfo>> contactInfoMockCollection = new List<Mock<IContactInfo>>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset, isLastYearOfStatusDate: true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2))
            };
            sut.Add(contactInfoMockCollection.Select(contactInfoMock => contactInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfo> contactInfoMock in contactInfoMockCollection)
            {
                if (contactInfoMock.Object.IsLastYearOfStatusDate)
                {
                    contactInfoMock.Verify(m => m.Year, Times.Once);
                    continue;
                }

                contactInfoMock.Verify(m => m.Year, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertMonthWasCalledOnEachContactInfoInContactInfoCollection()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<Mock<IContactInfo>> contactInfoMockCollection = new List<Mock<IContactInfo>>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset, isLastYearOfStatusDate: true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true),
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2))
            };
            sut.Add(contactInfoMockCollection.Select(contactInfoMock => contactInfoMock.Object).ToArray());

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IContactInfo> contactInfoMock in contactInfoMockCollection)
            {
                if (contactInfoMock.Object.IsLastYearOfStatusDate)
                {
                    contactInfoMock.Verify(m => m.Month, Times.Once);
                    continue;
                }

                contactInfoMock.Verify(m => m.Month, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsContactInfoCollection()
        {
            IContactInfoCollection sut = CreateSut();

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionDoesNotContainContactInfoForStatusDate_ReturnsContactInfoCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionDoesNotContainContactInfoForStatusDate_ReturnsContactInfoCollectionWhereBalanceInValuesAtStatusDateIsEqualToZero()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionContainsContactInfoForStatusDate_ReturnsContactInfoCollectionWhereValuesAtStatusDateIsNotNull()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IContactInfo contactInfoForStatusDate = _fixture.BuildContactInfoMock(contactInfoOffset, true).Object;
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                contactInfoForStatusDate,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionContainsContactInfoStatusDate_ReturnsContactInfoCollectionWhereBalanceInValuesAtStatusDateIsEqualToBalanceInContactInfoForStatusDate()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IContactInfo contactInfoForStatusDate = _fixture.BuildContactInfoMock(contactInfoOffset, true).Object;
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                contactInfoForStatusDate,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtStatusDate.Balance, Is.EqualTo(contactInfoForStatusDate.Balance));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionDoesNotContainContactInfoForLastMonthFromStatusDate_ReturnsContactInfoCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionDoesNotContainContactInfoForLastMonthFromStatusDate_ReturnsContactInfoCollectionWhereBalanceInValuesAtEndOfLastMonthFromStatusDateIsEqualToZero()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionContainsContactInfoForLastMonthFromStatusDate_ReturnsContactInfoCollectionWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IContactInfo contactInfoForLastMonthFromStatusDate = _fixture.BuildContactInfoMock(contactInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                contactInfoForLastMonthFromStatusDate,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionContainsContactInfoForLastMonthFromStatusDate_ReturnsContactInfoCollectionWhereBalanceInValuesAtEndOfLastMonthFromStatusDateIsEqualToBalanceInContactInfoForLastMonthFromStatusDate()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IContactInfo contactInfoForLastMonthFromStatusDate = _fixture.BuildContactInfoMock(contactInfoOffset, isLastMonthOfStatusDate: true).Object;
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                contactInfoForLastMonthFromStatusDate,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate.Balance, Is.EqualTo(contactInfoForLastMonthFromStatusDate.Balance));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionDoesNotContainContactInfoForEndOfLastYearFromStatusDate_ReturnsContactInfoCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionDoesNotContainContactInfoForEndOfLastYearFromStatusDate_ReturnsContactInfoCollectionWhereBalanceInValuesAtEndOfLastYearFromStatusDateIsEqualToZero()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                _fixture.BuildContactInfoMock(contactInfoOffset).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1)).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2)).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionContainsContactInfoForEndOfLastYearFromStatusDate_ReturnsContactInfoCollectionWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IContactInfo contactInfoForEndOfLastYearFromStatusDate = _fixture.BuildContactInfoMock(contactInfoOffset, isLastYearOfStatusDate: true).Object;
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                contactInfoForEndOfLastYearFromStatusDate,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2), isLastYearOfStatusDate: true).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenContactInfoCollectionContainsContactInfoForEndOfLastYearFromStatusDate_ReturnsContactInfoCollectionWhereBalanceInValuesAtEndOfLastYearFromStatusDateIsEqualToBalanceInContactInfoForEndOfLastYearFromStatusDate()
        {
            IContactInfoCollection sut = CreateSut();

            DateTime contactInfoOffset = DateTime.Today.AddDays(_random.Next(1, _random.Next(1, 365)) * -1);
            IContactInfo contactInfoForEndOfLastYearFromStatusDate = _fixture.BuildContactInfoMock(contactInfoOffset, isLastYearOfStatusDate: true).Object;
            IEnumerable<IContactInfo> contactInfoCollection = new List<IContactInfo>
            {
                contactInfoForEndOfLastYearFromStatusDate,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-1), isLastYearOfStatusDate: true).Object,
                _fixture.BuildContactInfoMock(contactInfoOffset.AddMonths(-2), isLastYearOfStatusDate: true).Object
            };
            sut.Add(contactInfoCollection);

            IContactInfoCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ValuesAtEndOfLastYearFromStatusDate.Balance, Is.EqualTo(contactInfoForEndOfLastYearFromStatusDate.Balance));
        }

        private IContactInfoCollection CreateSut()
        {
            return new Domain.Accounting.ContactInfoCollection();
        }
    }
}