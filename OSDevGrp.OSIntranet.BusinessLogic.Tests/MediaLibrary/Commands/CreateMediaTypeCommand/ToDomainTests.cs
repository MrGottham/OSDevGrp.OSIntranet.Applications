using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.CreateMediaTypeCommand
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
	        ICreateMediaTypeCommand sut = CreateSut();

            IMediaType result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaType()
        {
	        ICreateMediaTypeCommand sut = CreateSut();

            IMediaType result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<MediaType>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNumberIsEqualToNumberFromCreateMediaTypeCommand()
        {
            int number = _fixture.Create<int>();
            ICreateMediaTypeCommand sut = CreateSut(number);

            IMediaType result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNameIsNotNull()
        {
	        ICreateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNameIsNotEmpty()
        {
			ICreateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNameIsEqualToNameFromCreateMediaTypeCommand()
        {
            string name = _fixture.Create<string>();
			ICreateMediaTypeCommand sut = CreateSut(name: name);

            IMediaType result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereDeletableIsFalse()
        {
			ICreateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereCreatedByIdentifierIsNull()
        {
			ICreateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
			ICreateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereModifiedByIdentifierIsNull()
        {
			ICreateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
			ICreateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private ICreateMediaTypeCommand CreateSut(int? number = null, string name = null)
        {
            return MediaLibraryCommandFactory.BuildCreateMediaTypeCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}