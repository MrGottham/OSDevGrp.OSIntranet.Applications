using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.UpdateBookGenreCommand
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
	        IUpdateBookGenreCommand sut = CreateSut();

            IBookGenre result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenre()
        {
	        IUpdateBookGenreCommand sut = CreateSut();

            IBookGenre result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<BookGenre>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNumberIsEqualToNumberFromUpdateBookGenreCommand()
        {
            int number = _fixture.Create<int>();
            IUpdateBookGenreCommand sut = CreateSut(number);

            IBookGenre result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNameIsNotNull()
        {
	        IUpdateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNameIsNotEmpty()
        {
			IUpdateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNameIsEqualToNameFromUpdateBookGenreCommand()
        {
            string name = _fixture.Create<string>();
			IUpdateBookGenreCommand sut = CreateSut(name: name);

            IBookGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereDeletableIsFalse()
        {
			IUpdateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereCreatedByIdentifierIsNull()
        {
			IUpdateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
			IUpdateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereModifiedByIdentifierIsNull()
        {
			IUpdateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
			IUpdateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private IUpdateBookGenreCommand CreateSut(int? number = null, string name = null)
        {
            return MediaLibraryCommandFactory.BuildUpdateBookGenreCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}