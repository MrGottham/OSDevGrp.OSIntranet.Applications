using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupBase
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
            return new Sut(_fixture.Create<int>(), _fixture.Create<string>(), true);
        }

        private class Sut : Domain.Accounting.AccountGroupBase
        {
            #region Constructor

            public Sut(int number, string name, bool deletable)
                : base(number, name, deletable)
            {
            }

            #endregion
        }
    }
}