using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupStatus
{
    [TestFixture]
    public class ValuesAtEndOfLastMonthFromStatusDateTests
    {
        #region Properties

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastMonthFromStatusDate_WhenCalled_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IAccountGroupStatus sut = CreateSut(accountCollectionMock.Object);

            // ReSharper disable UnusedVariable
            IAccountCollectionValues result = sut.ValuesAtEndOfLastMonthFromStatusDate;
            // ReSharper restore UnusedVariable

            accountCollectionMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastMonthFromStatusDate_WhenCalled_ReturnsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            IAccountCollectionValues result = sut.ValuesAtEndOfLastMonthFromStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastMonthFromStatusDate_WhenCalled_ReturnsAccountCollectionValuesEqualToValuesAtEndOfLastMonthFromStatusDateFromAccountCollection()
        {
            IAccountCollectionValues valuesAtEndOfLastMonthFromStatusDate = _fixture.BuildAccountCollectionValuesMock().Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(valuesAtEndOfLastMonthFromStatusDate: valuesAtEndOfLastMonthFromStatusDate).Object;
            IAccountGroupStatus sut = CreateSut(accountCollection);

            IAccountCollectionValues result = sut.ValuesAtEndOfLastMonthFromStatusDate;

            Assert.That(result, Is.EqualTo(valuesAtEndOfLastMonthFromStatusDate));
        }

        private IAccountGroupStatus CreateSut(IAccountCollection accountCollection = null)
        {
            return new Domain.Accounting.AccountGroupStatus(_fixture.BuildAccountGroupMock().Object, accountCollection ?? _fixture.BuildAccountCollectionMock().Object);
        }
    }
}