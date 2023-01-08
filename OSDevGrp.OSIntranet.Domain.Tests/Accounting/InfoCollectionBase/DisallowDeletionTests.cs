using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoCollectionBase
{
    [TestFixture]
    public class DisallowDeletionTests
    {
        [Test]
        [Category("UnitTest")]
        public void DisallowDeletion_WhenCalled_ThrowsNotSupportedException()
        {
            IDeletable sut = CreateSut();

            NotSupportedException result = Assert.Throws<NotSupportedException>(sut.DisallowDeletion);

            Assert.That(result, Is.Not.Null);
        }

        private IDeletable CreateSut()
        {
            return new Sut();
        }

        private class Sut : Domain.Accounting.InfoCollectionBase<ICreditInfo, Sut>
        {
            #region Methods

            protected override Sut Calculate(DateTime statusDate, IReadOnlyCollection<ICreditInfo> calculatedInfoCollection) => throw new NotSupportedException();

            protected override Sut AlreadyCalculated() => throw new NotSupportedException();

            #endregion
        }
    }
}
