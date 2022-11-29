using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
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
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.Throws<NotSupportedException>(() => sut.AllowDeletion());
        }

        private IBudgetAccountGroupStatus CreateSut()
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(_fixture.BuildBudgetAccountGroupMock().Object, _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}