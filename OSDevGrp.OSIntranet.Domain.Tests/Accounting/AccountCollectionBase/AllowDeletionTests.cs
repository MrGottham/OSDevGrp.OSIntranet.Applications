using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountCollectionBase
{
    [TestFixture]
    public class AllowDeletionTests
    {
        [Test]
        [Category("UnitTest")]
        public void AllowDeletion_WhenCalled_ThrowsNotSupportedException()
        {
            IDeletable sut = CreateSut();

            NotSupportedException result = Assert.Throws<NotSupportedException>(sut.AllowDeletion);

            Assert.That(result, Is.Not.Null);
        }

        private IDeletable CreateSut()
        {
            return new Sut();
        }

        private class Sut : Domain.Accounting.AccountCollectionBase<IAccount, Sut>
        {
            #region Methods

            protected override Sut Calculate(DateTime statusDate, IReadOnlyCollection<IAccount> calculatedAccountCollection) => throw new NotSupportedException();

            protected override Sut AlreadyCalculated() => throw new NotSupportedException();

            #endregion
        }
    }
}