using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactAccount
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
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnContactInfoCollection()
        {
            Mock<IContactInfoCollection> contactInfoCollectionMock = _fixture.BuildContactInfoCollectionMock();
            IProtectable sut = CreateSut(contactInfoCollectionMock.Object);

            sut.ApplyProtection();

            contactInfoCollectionMock.Verify(m => m.ApplyProtection(), Times.Once);
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

        private IContactAccount CreateSut(IContactInfoCollection contactInfoCollection = null, IPostingLineCollection postingLineCollection = null)
        {
            return new Domain.Accounting.ContactAccount(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildPaymentTermMock().Object, contactInfoCollection ?? _fixture.BuildContactInfoCollectionMock().Object, postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}