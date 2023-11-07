using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBase
{
	[TestFixture]
    public class DisallowDeletionTests
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
        public void DisallowDeletion_WhenCalled_AssertDeletableIsFalseOnMediaBase(bool deletable)
        {
            IDeletable sut = CreateSut(deletable);

            Assert.That(sut.Deletable, Is.EqualTo(deletable));

            sut.DisallowDeletion();

            Assert.That(sut.Deletable, Is.False);
        }

        private IDeletable CreateSut(bool deletable)
        {
            return new MyMedia(Guid.NewGuid(), _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null,_fixture.BuildMediaTypeMock().Object, _random.Next(100) > 50 ? null : _fixture.Create<short>(),  _random.Next(100) > 50 ? new Uri($"https://localhost/api/medias/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, _ => Array.Empty<IMediaBinding>(), _ => Array.Empty<ILending>(), deletable);
        }

        private class MyMedia : Domain.MediaLibrary.MediaBase
        {
            #region Constructor

            public MyMedia(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMediaType mediaType, short? published, Uri url, byte[] image, Func<IMedia, IEnumerable<IMediaBinding>> mediaBindingsBuilder, Func<IMedia, IEnumerable<ILending>> lendingsBuilder, bool deletable)
                : base(mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, mediaBindingsBuilder, lendingsBuilder, deletable)
            {
            }

            #endregion
        }
    }
}