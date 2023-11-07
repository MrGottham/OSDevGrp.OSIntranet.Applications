using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.UpdateMusicGenreCommand
{
    [TestFixture]
    public class ToDomainTests
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
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
	        IUpdateMusicGenreCommand sut = CreateSut();

            IMusicGenre result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenre()
        {
	        IUpdateMusicGenreCommand sut = CreateSut();

            IMusicGenre result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<MusicGenre>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNumberIsEqualToNumberFromUpdateMusicGenreCommand()
        {
            int number = _fixture.Create<int>();
            IUpdateMusicGenreCommand sut = CreateSut(number);

            IMusicGenre result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNameIsNotNull()
        {
	        IUpdateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNameIsNotEmpty()
        {
			IUpdateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNameIsEqualToNameFromUpdateMusicGenreCommand()
        {
            string name = _fixture.Create<string>();
			IUpdateMusicGenreCommand sut = CreateSut(name: name);

            IMusicGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereDeletableIsFalse()
        {
			IUpdateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereCreatedByIdentifierIsNull()
        {
			IUpdateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
			IUpdateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereModifiedByIdentifierIsNull()
        {
			IUpdateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
			IUpdateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private IUpdateMusicGenreCommand CreateSut(int? number = null, string name = null)
        {
            return MediaLibraryCommandFactory.BuildUpdateMusicGenreCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}