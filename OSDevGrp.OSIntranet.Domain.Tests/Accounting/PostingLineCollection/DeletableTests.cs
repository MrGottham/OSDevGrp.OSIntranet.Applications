using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLineCollection
{
    [TestFixture]
    public class DeletableTests
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
        public void Deletable_WhenNoPostingLineWasAdded_ReturnsFalse()
        {
            IPostingLineCollection sut = CreateSut();

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenOnePostingLineWasAdded_ReturnsFalse()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.BuildPostingLineMock().Object);

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenSomePostingLinesWasAdded_ReturnsFalse()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.BuildPostingLineMock().Object);
            sut.Add(_fixture.BuildPostingLineMock().Object);
            sut.Add(_fixture.BuildPostingLineMock().Object);

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}