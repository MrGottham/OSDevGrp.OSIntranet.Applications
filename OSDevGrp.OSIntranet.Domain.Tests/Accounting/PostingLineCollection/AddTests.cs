using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLineCollection
{
    [TestFixture]
    public class AddTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingLineIsNull_ThrowsArgumentNullException()
        {
            IPostingLineCollection sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((IPostingLine) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingLine"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingLineIsNotNullAndDoesNotExistWithinCollection_AddsPostingLineToCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLine postingLine = _fixture.BuildPostingLineMock().Object;
            sut.Add(postingLine);

            Assert.That(sut.Contains(postingLine), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingLineIsNotNullAndDoesExistWithinCollection_ThrowsIntranetSystemException()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLine postingLine = _fixture.BuildPostingLineMock().Object;
            sut.Add(postingLine);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Add(postingLine));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains(postingLine.GetType().Name), Is.True);
            // ReSharper restore PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ObjectAlreadyExists));
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingLineCollectionIsNull_ThrowsArgumentNullException()
        {
            IPostingLineCollection sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((IEnumerable<IPostingLine>) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingLineCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingLineCollectionIsNotNullAndEachPostingLineDoesNotExistWithinCollection_AddsEachPostingLineToCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<IPostingLine> postingLineCollection = new List<IPostingLine>
            {
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object
            };
            sut.Add(postingLineCollection);

            Assert.That(postingLineCollection.All(account => sut.Contains(account)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenPostingLineCollectionIsNotNullAndOnePostingLineDoesExistWithinCollection_ThrowsIntranetSystemException()
        {
            IPostingLineCollection sut = CreateSut();

            IList<IPostingLine> postingLineCollection = new List<IPostingLine>
            {
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object
            };

            IPostingLine existingPostingLine = postingLineCollection[_random.Next(0, postingLineCollection.Count - 1)];
            sut.Add(existingPostingLine);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Add(postingLineCollection));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains(existingPostingLine.GetType().Name), Is.True);
            // ReSharper restore PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ObjectAlreadyExists));
            Assert.That(result.InnerException, Is.Null);
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}