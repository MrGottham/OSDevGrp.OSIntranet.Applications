using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupStatus
{
    [TestFixture]
    public class ValuesAtStatusDateTests
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
        public void ValuesAtStatusDate_WhenCalled_AssertValuesAtStatusDateWasCalledOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IAccountGroupStatus sut = CreateSut(accountCollectionMock.Object);

            // ReSharper disable UnusedVariable
            IAccountCollectionValues result = sut.ValuesAtStatusDate;
            // ReSharper restore UnusedVariable

            accountCollectionMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtStatusDate_WhenCalled_ReturnsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            IAccountCollectionValues result = sut.ValuesAtStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtStatusDate_WhenCalled_ReturnsAccountCollectionValuesEqualToValuesAtStatusDateFromAccountCollection()
        {
            IAccountCollectionValues valuesAtStatusDate = _fixture.BuildAccountCollectionValuesMock().Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(valuesAtStatusDate: valuesAtStatusDate).Object;
            IAccountGroupStatus sut = CreateSut(accountCollection);

            IAccountCollectionValues result = sut.ValuesAtStatusDate;

            Assert.That(result, Is.EqualTo(valuesAtStatusDate));
        }

        private IAccountGroupStatus CreateSut(IAccountCollection accountCollection = null)
        {
            return new Domain.Accounting.AccountGroupStatus(_fixture.BuildAccountGroupMock().Object, accountCollection ?? _fixture.BuildAccountCollectionMock().Object);
        }
    }
}