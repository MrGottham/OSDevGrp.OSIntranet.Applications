using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Tests.Core.GenericCategoryBase
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
        [TestCase(true)]
        [TestCase(false)]
        public void DisallowDeletion_WhenCalled_AssertDeletableIsFalseOnGenericCategoryBase(bool deletable)
        {
            IDeletable sut = CreateSut(deletable);

            Assert.That(sut.Deletable, Is.EqualTo(deletable));

            sut.DisallowDeletion();

            Assert.That(sut.Deletable, Is.False);
        }

        private IDeletable CreateSut(bool deletable)
        {
            return new MyGenericCategory(_fixture.Create<int>(), _fixture.Create<string>(), deletable);
        }

        private class MyGenericCategory : Domain.Core.GenericCategoryBase
        {
            #region Constructor

            public MyGenericCategory(int number, string name, bool deletable)
                : base(number, name, deletable)
            {
            }

            #endregion
        }
    }
}