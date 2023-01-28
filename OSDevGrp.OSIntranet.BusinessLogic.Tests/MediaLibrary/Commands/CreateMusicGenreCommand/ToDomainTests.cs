using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.CreateMusicGenreCommand
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
	        ICreateMusicGenreCommand sut = CreateSut();

            IMusicGenre result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenre()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

            IMusicGenre result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<MusicGenre>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNumberIsEqualToNumberFromCreateMusicGenreCommand()
        {
            int number = _fixture.Create<int>();
            ICreateMusicGenreCommand sut = CreateSut(number);

            IMusicGenre result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNameIsNotNull()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNameIsNotEmpty()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereNameIsEqualToNameFromCreateMusicGenreCommand()
        {
            string name = _fixture.Create<string>();
            ICreateMusicGenreCommand sut = CreateSut(name: name);

            IMusicGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereDeletableIsFalse()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereCreatedByIdentifierIsNull()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereModifiedByIdentifierIsNull()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMusicGenreWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
	        ICreateMusicGenreCommand sut = CreateSut();

	        IMusicGenre result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private ICreateMusicGenreCommand CreateSut(int? number = null, string name = null)
        {
            return MediaLibraryCommandFactory.BuildCreateMusicGenreCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}