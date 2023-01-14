using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBase
{
    [TestFixture]
    public class AllowDeletionTests
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
        [TestCase(true)]
        [TestCase(false)]
        public void AllowDeletion_WhenCalled_AssertDeletableIsTrueOnMediaBase(bool deletable)
        {
            IDeletable sut = CreateSut(deletable);

            Assert.That(sut.Deletable, Is.EqualTo(deletable));

            sut.AllowDeletion();

            Assert.That(sut.Deletable, Is.True);
        }

        private IDeletable CreateSut(bool deletable)
        {
            return new MyMedia(Guid.NewGuid(), _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.BuildMediaTypeMock().Object, _random.Next(100) > 50 ? null : _fixture.Create<short>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, deletable);
        }

        private class MyMedia : Domain.MediaLibrary.MediaBase
        {
            #region Constructor

            public MyMedia(Guid mediaIdentifier, string title, string subtitle, string description, IMediaType mediaType, short? published, string details, byte[] image, bool deletable)
                : base(mediaIdentifier, title, subtitle, description, mediaType, published, details, image, deletable)
            {
            }

            #endregion
        }
    }
}