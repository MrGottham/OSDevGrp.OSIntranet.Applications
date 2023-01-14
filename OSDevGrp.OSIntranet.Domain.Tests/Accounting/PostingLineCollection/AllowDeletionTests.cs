using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLineCollection
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
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}