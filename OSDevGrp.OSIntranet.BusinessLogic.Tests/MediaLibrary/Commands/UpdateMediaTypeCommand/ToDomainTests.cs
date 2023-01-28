using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.UpdateMediaTypeCommand
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
	        IUpdateMediaTypeCommand sut = CreateSut();

            IMediaType result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaType()
        {
	        IUpdateMediaTypeCommand sut = CreateSut();

            IMediaType result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<MediaType>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNumberIsEqualToNumberFromUpdateMediaTypeCommand()
        {
            int number = _fixture.Create<int>();
            IUpdateMediaTypeCommand sut = CreateSut(number);

            IMediaType result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNameIsNotNull()
        {
	        IUpdateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNameIsNotEmpty()
        {
			IUpdateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereNameIsEqualToNameFromUpdateMediaTypeCommand()
        {
            string name = _fixture.Create<string>();
			IUpdateMediaTypeCommand sut = CreateSut(name: name);

            IMediaType result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereDeletableIsFalse()
        {
			IUpdateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereCreatedByIdentifierIsNull()
        {
			IUpdateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
			IUpdateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereModifiedByIdentifierIsNull()
        {
			IUpdateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsMediaTypeWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
			IUpdateMediaTypeCommand sut = CreateSut();

	        IMediaType result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private IUpdateMediaTypeCommand CreateSut(int? number = null, string name = null)
        {
            return MediaLibraryCommandFactory.BuildUpdateMediaTypeCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}