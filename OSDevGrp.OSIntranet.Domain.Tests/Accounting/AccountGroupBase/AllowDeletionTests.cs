using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupBase
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
            return new Sut(_fixture.Create<int>(), _fixture.Create<string>(), false);
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