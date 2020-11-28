using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Account
{
    [TestFixture]
    public class AccountGroupTypeTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AccountGroupType_WhenCalled_AssertAccountGroupTypeWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            IAccount sut = CreateSut(accountGroupMock.Object);

            AccountGroupType result = sut.AccountGroupType;
            Assert.That(result, Is.AnyOf(AccountGroupType.Assets, AccountGroupType.Liabilities));

            accountGroupMock.Verify(m => m.AccountGroupType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(AccountGroupType.Assets)]
        [TestCase(AccountGroupType.Liabilities)]
        public void AccountGroupType_WhenCalled_ReturnsAccountGroupTypeFromAccountGroup(AccountGroupType accountGroupType)
        {
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(accountGroupType: accountGroupType).Object;
            IAccount sut = CreateSut(accountGroup);

            AccountGroupType result = sut.AccountGroupType;
            
            Assert.That(result, Is.EqualTo(accountGroupType));
        }

        private IAccount CreateSut(IAccountGroup accountGroup = null)
        {
            return new Domain.Accounting.Account(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), accountGroup ?? _fixture.BuildAccountGroupMock().Object);
        }
    }
}