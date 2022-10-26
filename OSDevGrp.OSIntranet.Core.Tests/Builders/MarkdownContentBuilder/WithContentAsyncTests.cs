using System;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Tests.Builders.MarkdownContentBuilder
{
    [TestFixture]
    public class WithContentAsyncTests
    {
        #region Private variables

        private Mock<IDomainObjectToMarkdownConverter<IEvent>> _domainObjectToMarkdownConverterMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _domainObjectToMarkdownConverterMock = new Mock<IDomainObjectToMarkdownConverter<IEvent>>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithContentAsync<IExportQuery, object>(null, CreateDomainObject()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenDataIsNull_ThrowsArgumentNullException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithContentAsync<IExportQuery, object>(CreateExportQuery(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("data"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenDataCantBeCastToDomainObject_ThrowsArgumentException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            ArgumentException result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.WithContentAsync(CreateExportQuery(), _fixture.Create<object>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.EqualTo($"Value cannot be cast to {nameof(IEvent)}. (Parameter 'data')"));
            Assert.That(result.ParamName, Is.EqualTo("data"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenCalled_AssertConvertAsyncWasCalledOnDomainObjectToMarkdownConverter()
        {
            using IExportDataContentBuilder sut = CreateSut();

            IEvent domainObject = CreateDomainObject();
            await sut.WithContentAsync(CreateExportQuery(), domainObject);

            _domainObjectToMarkdownConverterMock.Verify(m => m.ConvertAsync(It.Is<IEvent>(value => value != null && value == domainObject)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenNoMarkdownContentWasReturnedFromConvertAsyncOnDomainObjectToMarkdownConverter_AssertNoWrittenData()
        {
            using IExportDataContentBuilder sut = CreateSut(false);

            await sut.WithContentAsync(CreateExportQuery(), CreateDomainObject());

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenMarkdownContentWasReturnedFromConvertAsyncOnDomainObjectToMarkdownConverter_AssertMarkdownContentHasBeenWritten()
        {
            string markdownContent = _fixture.Create<string>();
            using IExportDataContentBuilder sut = CreateSut(markdownContent: markdownContent);

            await sut.WithContentAsync(CreateExportQuery(), CreateDomainObject());

            byte[] result = await sut.BuildAsync();

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(markdownContent));
        }

        private IExportDataContentBuilder CreateSut(bool hasMarkdownContent = true, string markdownContent = null)
        {
            _domainObjectToMarkdownConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IEvent>()))
                .Returns(Task.FromResult(hasMarkdownContent ? markdownContent ?? _fixture.Create<string>() : null));

            return new MarkdownContentBuilder<IEvent, IDomainObjectToMarkdownConverter<IEvent>>(_domainObjectToMarkdownConverterMock.Object, false);
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }

        private IEvent CreateDomainObject()
        {
            return new Mock<IEvent>().Object;
        }
    }
}