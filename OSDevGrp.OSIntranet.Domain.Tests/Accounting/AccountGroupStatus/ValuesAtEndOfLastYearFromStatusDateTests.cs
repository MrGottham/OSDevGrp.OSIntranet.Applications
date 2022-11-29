using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupStatus
{
    [TestFixture]
    public class ValuesAtEndOfLastYearFromStatusDateTests
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
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IAccountGroupStatus sut = CreateSut(accountCollectionMock.Object);

            // ReSharper disable UnusedVariable
            IAccountCollectionValues result = sut.ValuesAtEndOfLastYearFromStatusDate;
            // ReSharper restore UnusedVariable

            accountCollectionMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_ReturnsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            IAccountCollectionValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_ReturnsAccountCollectionValuesEqualToValuesAtEndOfLastYearFromStatusDateFromAccountCollection()
        {
            IAccountCollectionValues valuesAtEndOfLastYearFromStatusDate = _fixture.BuildAccountCollectionValuesMock().Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(valuesAtEndOfLastYearFromStatusDate: valuesAtEndOfLastYearFromStatusDate).Object;
            IAccountGroupStatus sut = CreateSut(accountCollection);

            IAccountCollectionValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.EqualTo(valuesAtEndOfLastYearFromStatusDate));
        }

        private IAccountGroupStatus CreateSut(IAccountCollection accountCollection = null)
        {
            return new Domain.Accounting.AccountGroupStatus(_fixture.BuildAccountGroupMock().Object, accountCollection ?? _fixture.BuildAccountCollectionMock().Object);
        }
    }
}