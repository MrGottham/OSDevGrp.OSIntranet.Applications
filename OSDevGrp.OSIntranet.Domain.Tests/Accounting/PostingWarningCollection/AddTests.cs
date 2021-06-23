using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingWarningCollection
{
    [TestFixture]
    public class AddTests
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
        public void Add_WhenPostingWarningIsNull_ThrowsArgumentNullException()
        {
            IPostingWarningCollection sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((IPostingWarning) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingWarning"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingWarningIsNotNull_AddsPostingWarningToCollection()
        {
            IPostingWarningCollection sut = CreateSut();

            IPostingWarning postingWarning = _fixture.BuildPostingWarningMock().Object;
            sut.Add(postingWarning);

            Assert.That(sut.Contains(postingWarning), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingWarningCollectionIsNull_ThrowsArgumentNullException()
        {
            IPostingWarningCollection sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((IEnumerable<IPostingWarning>) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingWarningCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingWarningIsNotNull_AddsEachPostingWarningToCollection()
        {
            IPostingWarningCollection sut = CreateSut();

            IEnumerable<IPostingWarning> postingWarningCollection = new List<IPostingWarning>
            {
                _fixture.BuildPostingWarningMock().Object,
                _fixture.BuildPostingWarningMock().Object,
                _fixture.BuildPostingWarningMock().Object,
                _fixture.BuildPostingWarningMock().Object,
                _fixture.BuildPostingWarningMock().Object,
                _fixture.BuildPostingWarningMock().Object,
                _fixture.BuildPostingWarningMock().Object
            };
            sut.Add(postingWarningCollection);

            Assert.That(postingWarningCollection.All(account => sut.Contains(account)), Is.True);
        }

        private IPostingWarningCollection CreateSut()
        {
            return new Domain.Accounting.PostingWarningCollection();
        }
    }
}