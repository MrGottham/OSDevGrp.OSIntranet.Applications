using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccount
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
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnBudgetInfoCollection()
        {
            Mock<IBudgetInfoCollection> budgetInfoCollectionMock = _fixture.BuildBudgetInfoCollectionMock();
            IProtectable sut = CreateSut(budgetInfoCollectionMock.Object);

            sut.ApplyProtection();

            budgetInfoCollectionMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IProtectable sut = CreateSut(postingLineCollection: postingLineCollectionMock.Object);

            sut.ApplyProtection();

            postingLineCollectionMock.Verify(m => m.ApplyProtection(), Times.Once);
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

        private IBudgetAccount CreateSut(IBudgetInfoCollection budgetInfoCollection = null, IPostingLineCollection postingLineCollection = null)
        {
            return new Domain.Accounting.BudgetAccount(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildBudgetAccountGroupMock().Object, budgetInfoCollection ?? _fixture.BuildBudgetInfoCollectionMock().Object, postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}