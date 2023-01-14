using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountBase
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
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IProtectable sut = CreateSut(postingLineCollectionMock.Object);

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

        private IAccountBase CreateSut(IPostingLineCollection postingLineCollection = null)
        {
            return new Sut(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object);
        }

        private class Sut : Domain.Accounting.AccountBase<IAccountBase>
        {
            #region Constructor

            public Sut(IAccounting accounting, string accountNumber, string accountName, IPostingLineCollection postingLineCollection)
                : base(accounting, accountNumber, accountName, postingLineCollection)
            {
            }

            #endregion

            #region Methods

            protected override Task[] GetCalculationTasks(DateTime statusDate) => throw new NotSupportedException();

            protected override Task<IAccountBase> GetCalculationResultAsync() => throw new NotSupportedException();

            protected override IAccountBase AlreadyCalculated() => throw new NotSupportedException();

            #endregion
        }
    }
}