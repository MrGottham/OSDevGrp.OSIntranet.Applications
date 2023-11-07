using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.CreateBookGenreCommand
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
	        ICreateBookGenreCommand sut = CreateSut();

            IBookGenre result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenre()
        {
	        ICreateBookGenreCommand sut = CreateSut();

            IBookGenre result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<BookGenre>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNumberIsEqualToNumberFromCreateBookGenreCommand()
        {
            int number = _fixture.Create<int>();
            ICreateBookGenreCommand sut = CreateSut(number);

            IBookGenre result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNameIsNotNull()
        {
	        ICreateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNameIsNotEmpty()
        {
			ICreateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereNameIsEqualToNameFromCreateBookGenreCommand()
        {
            string name = _fixture.Create<string>();
			ICreateBookGenreCommand sut = CreateSut(name: name);

            IBookGenre result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereDeletableIsFalse()
        {
			ICreateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereCreatedByIdentifierIsNull()
        {
			ICreateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
			ICreateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereModifiedByIdentifierIsNull()
        {
			ICreateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBookGenreWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
			ICreateBookGenreCommand sut = CreateSut();

	        IBookGenre result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private ICreateBookGenreCommand CreateSut(int? number = null, string name = null)
        {
            return MediaLibraryCommandFactory.BuildCreateBookGenreCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}