using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using Sut=OSDevGrp.OSIntranet.Domain.Core.DeletableExtensions;

namespace OSDevGrp.OSIntranet.Domain.Tests.Core.DeletableExtensions
{
    [TestFixture]
    public class SetDeletableTests
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
        public void SetDeletable_WhenDeletableIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.SetDeletable(null, _fixture.Create<bool>()));

            Assert.That(result.ParamName, Is.EqualTo("deletable"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetDeletable_WhenCanDeleteIsTrue_AssertAllowDeletionWasCalledOnDeletable()
        {
            Mock<IDeletable> deletableMock = _fixture.BuildDeletableMock();
            
            Sut.SetDeletable(deletableMock.Object, true);

            deletableMock.Verify(m => m.AllowDeletion(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void SetDeletable_WhenCanDeleteIsTrue_AssertDisallowDeletionWasCalledOnDeletable()
        {
            Mock<IDeletable> deletableMock = _fixture.BuildDeletableMock();
            
            Sut.SetDeletable(deletableMock.Object, true);

            deletableMock.Verify(m => m.DisallowDeletion(), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void SetDeletable_WhenCanDeleteIsFalse_AssertDisallowDeletionWasCalledOnDeletable()
        {
            Mock<IDeletable> deletableMock = _fixture.BuildDeletableMock();
            
            Sut.SetDeletable(deletableMock.Object, false);

            deletableMock.Verify(m => m.DisallowDeletion(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void SetDeletable_WhenCanDeleteIsFalse_AssertAllowDeletionWasNotCalledOnDeletable()
        {
            Mock<IDeletable> deletableMock = _fixture.BuildDeletableMock();
            
            Sut.SetDeletable(deletableMock.Object, false);

            deletableMock.Verify(m => m.AllowDeletion(), Times.Never());
        }
   }
}