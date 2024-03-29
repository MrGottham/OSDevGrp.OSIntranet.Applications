using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountBase
{
    [TestFixture]
    public class DisallowDeletionTests
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
        public void DisallowDeletion_WhenCalled_AssertDeletableIsFalse()
        {
            IDeletable sut = CreateSut();

            sut.DisallowDeletion();

            Assert.That(sut.Deletable, Is.False);
        }

        private IDeletable CreateSut()
        {
            return new Sut(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildPostingLineCollectionMock().Object);
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