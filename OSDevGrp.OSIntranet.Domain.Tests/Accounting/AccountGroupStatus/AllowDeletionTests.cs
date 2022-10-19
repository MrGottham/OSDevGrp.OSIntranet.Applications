using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupStatus
{
    [TestFixture]
    public class AllowDeletionTests
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
        public void AllowDeletion_WhenCalled_ThrowsNotSupportedException()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.Throws<NotSupportedException>(() => sut.AllowDeletion());
        }

        private IAccountGroupStatus CreateSut()
        {
            return new Domain.Accounting.AccountGroupStatus(_fixture.BuildAccountGroupMock().Object, _fixture.BuildAccountCollectionMock().Object);
        }
    }
}