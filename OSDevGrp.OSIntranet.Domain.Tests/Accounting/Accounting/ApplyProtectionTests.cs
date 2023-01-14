using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Accounting
{
    [TestFixture]
    public class ApplyProtectionTests
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
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IProtectable sut = CreateSut(accountCollectionMock.Object);

            sut.ApplyProtection();

            accountCollectionMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IProtectable sut = CreateSut(budgetAccountCollection: budgetAccountCollectionMock.Object);

            sut.ApplyProtection();

            budgetAccountCollectionMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnContactAccountCollection()
        {
            Mock<IContactAccountCollection> contactAccountCollectionMock = _fixture.BuildContactAccountCollectionMock();
            IProtectable sut = CreateSut(contactAccountCollection: contactAccountCollectionMock.Object);

            sut.ApplyProtection();

            contactAccountCollectionMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertIsProtectedIsTrue()
        {
            IProtectable sut = CreateSut();

            Assert.That(sut.IsProtected, Is.False);

            sut.ApplyProtection();

            Assert.That(sut.IsProtected, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertDeletableIsFalse()
        {
            IProtectable sut = CreateSut();
            sut.AllowDeletion();

            Assert.That(sut.Deletable, Is.True);

            sut.ApplyProtection();

            Assert.That(sut.Deletable, Is.False);
        }

        private IProtectable CreateSut(IAccountCollection accountCollection = null, IBudgetAccountCollection budgetAccountCollection = null, IContactAccountCollection contactAccountCollection = null)
        {
            return new Domain.Accounting.Accounting(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.BuildLetterHeadMock().Object, _fixture.Create<BalanceBelowZeroType>(), _fixture.Create<int>(), accountCollection ?? _fixture.BuildAccountCollectionMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object, contactAccountCollection ?? _fixture.BuildContactAccountCollectionMock().Object);
        }
    }
}