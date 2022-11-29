using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoFixture;
using Markdown;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Tests.Converters.DomainObjectToMarkdownConverterBase
{
    [TestFixture]
    public class ConvertAsyncTests
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
        public void ConvertAsync_WhenDomainObjectIsNull_ArgumentNullException()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertWriteDocumentAsyncCalledWasCalledOnDomainObjectToMarkdownConverterBase()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            await sut.ConvertAsync(_fixture.Create<object>());

            Assert.That(((MyDomainObjectToMarkdownConverter)sut).WriteDocumentAsyncHasBeenCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertWriteDocumentAsyncCalledWasCalledOnDomainObjectToMarkdownConverterBaseWithGivenDomainObject()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            object domainObject = _fixture.Create<object>();
            await sut.ConvertAsync(domainObject);

            Assert.That(((MyDomainObjectToMarkdownConverter)sut).WriteDocumentAsyncCalledWithDomainObject, Is.EqualTo(domainObject));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenNoMarkdownWasWrittenForDomainObject_ReturnsNull()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut(false);

            string result = await sut.ConvertAsync(_fixture.Create<object>());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenMarkdownWasWrittenForDomainObject_ReturnsNotNull()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.Create<object>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenMarkdownWasWrittenForDomainObject_ReturnsNotEmpty()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.Create<object>());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenMarkdownWasWrittenForDomainObject_ReturnsMarkdownForMarkdownDocument()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            string value = _fixture.Create<string>();
            string result = await sut.ConvertAsync(value);

            Assert.That(result, Is.EqualTo($"# {value}{Environment.NewLine}"));
        }

        private IDomainObjectToMarkdownConverter<object> CreateSut(bool writeMarkdownForDomainObject = true)
        {
            return new MyDomainObjectToMarkdownConverter(writeMarkdownForDomainObject, CultureInfo.InvariantCulture);
        }

        private class MyDomainObjectToMarkdownConverter : DomainObjectToMarkdownConverterBase<object>
        {
            #region Private variables

            private readonly bool _writeMarkdownForDomainObject;

            #endregion

            #region Constructors

            public MyDomainObjectToMarkdownConverter(bool writeMarkdownForDomainObject, IFormatProvider formatProvider)
                : base(formatProvider)
            {
                _writeMarkdownForDomainObject = writeMarkdownForDomainObject;
            }

            #endregion

            #region Properties

            public bool WriteDocumentAsyncHasBeenCalled { get; private set; }

            public object WriteDocumentAsyncCalledWithDomainObject { get; private set; }

            #endregion

            #region Methods

            protected override Task WriteDocumentAsync(object domainObject, IMarkdownDocument markdownDocument)
            {
                Core.NullGuard.NotNull(domainObject, nameof(domainObject))
                    .NotNull(markdownDocument, nameof(markdownDocument));

                WriteDocumentAsyncHasBeenCalled = true;
                WriteDocumentAsyncCalledWithDomainObject = domainObject;

                if (_writeMarkdownForDomainObject)
                {
                    markdownDocument.AppendHeader(domainObject.ToString(), 1);
                }

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}