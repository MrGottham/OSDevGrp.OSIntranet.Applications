using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.UpdateMovieGenreCommand
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
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenre()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<MovieGenre>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereNumberIsEqualToNumberFromUpdateMovieGenreCommand()
        {
            int number = _fixture.Create<int>();
            IUpdateMovieGenreCommand sut = CreateSut(number);

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereNameIsNotNull()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereNameIsNotEmpty()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereNameIsEqualToNameFromUpdateMovieGenreCommand()
        {
            string name = _fixture.Create<string>();
            IUpdateMovieGenreCommand sut = CreateSut(name: name);

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereDeletableIsFalse()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereCreatedByIdentifierIsNull()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereModifiedByIdentifierIsNull()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMovieGenreWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
            IUpdateMovieGenreCommand sut = CreateSut();

            IMovieGenre result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private IUpdateMovieGenreCommand CreateSut(int? number = null, string name = null)
        {
            return MediaLibraryCommandFactory.BuildUpdateMovieGenreCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}