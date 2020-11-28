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
    public class AllowDeletionTests
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
        public void AllowDeletion_WhenCalled_AssertDeletableIsTrue()
        {
            IDeletable sut = CreateSut();

            sut.AllowDeletion();

            Assert.That(sut.Deletable, Is.True);
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

            protected override IAccountBase GetCalculationResult() => throw new NotSupportedException();

            #endregion
        }
    }
}