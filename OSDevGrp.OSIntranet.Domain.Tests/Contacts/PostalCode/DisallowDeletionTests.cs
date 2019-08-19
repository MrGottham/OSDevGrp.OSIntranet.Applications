using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.PostalCode
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
            return new Domain.Contacts.PostalCode(_fixture.BuildCountryMock().Object, _fixture.Create<string>(), _fixture.Create<string>());
        }
    }
}
