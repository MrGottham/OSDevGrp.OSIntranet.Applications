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
    public class GetCreatorInformationMarkdownTests
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
        public void GetCreatorInformationMarkdown_WhenCreatorNameAndCreatorMailAddressIsGiven_ReturnsNotNull()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), GetCreatorName(), GetCreatorMailAddress());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetCreatorInformationMarkdown_WhenCreatorNameAndCreatorMailAddressIsGiven_ReturnsMarkdownParagraph()
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), GetCreatorName(), GetCreatorMailAddress());

            Assert.That(result, Is.TypeOf<MarkdownParagraph>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetCreatorInformationMarkdown_WhenCreatorNameAndCreatorMailAddressIsGiven_ReturnsMarkdownParagraphForCreatorInformation()
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            IDomainObjectToMarkdownConverter<object> sut = CreateSut(formatProvider);

            DateTime creationTime = GetCreationTime();
            string creatorName = GetCreatorName();
            string creatorMailAddress = GetCreatorMailAddress();
            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(creationTime, creatorName, creatorMailAddress);

            Assert.That(result.ToString(), Is.EqualTo($"*Udskrevet den {creationTime.ToString("d", formatProvider)} kl. {creationTime.ToString("t", formatProvider)} af [{creatorName}](mailto:{creatorMailAddress})*{Environment.NewLine}"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorNameIsGiven_ReturnsNotNull(string creatorMailAddress)
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), GetCreatorName(), creatorMailAddress);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorNameIsGiven_ReturnsMarkdownParagraph(string creatorMailAddress)
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), GetCreatorName(), creatorMailAddress);

            Assert.That(result, Is.TypeOf<MarkdownParagraph>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorNameIsGiven_ReturnsMarkdownParagraphForCreatorInformation(string creatorMailAddress)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            IDomainObjectToMarkdownConverter<object> sut = CreateSut(formatProvider);

            DateTime creationTime = GetCreationTime();
            string creatorName = GetCreatorName();
            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(creationTime, creatorName, creatorMailAddress);

            Assert.That(result.ToString(), Is.EqualTo($"*Udskrevet den {creationTime.ToString("d", formatProvider)} kl. {creationTime.ToString("t", formatProvider)} af {creatorName}*{Environment.NewLine}"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorMailAddressIsGiven_ReturnsNotNull(string creatorName)
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), creatorName, GetCreatorMailAddress());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorMailAddressIsGiven_ReturnsMarkdownParagraph(string creatorName)
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), creatorName, GetCreatorMailAddress());

            Assert.That(result, Is.TypeOf<MarkdownParagraph>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorMailAddressIsGiven_ReturnsMarkdownParagraphForCreatorInformation(string creatorName)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            IDomainObjectToMarkdownConverter<object> sut = CreateSut(formatProvider);

            DateTime creationTime = GetCreationTime();
            string creatorMailAddress = GetCreatorMailAddress();
            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(creationTime, creatorName, creatorMailAddress);

            Assert.That(result.ToString(), Is.EqualTo($"*Udskrevet den {creationTime.ToString("d", formatProvider)} kl. {creationTime.ToString("t", formatProvider)} af [{creatorMailAddress}](mailto:{creatorMailAddress})*{Environment.NewLine}"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(" ", null)]
        [TestCase("  ", null)]
        [TestCase("   ", null)]
        [TestCase(null, "")]
        [TestCase(null, " ")]
        [TestCase(null, "  ")]
        [TestCase(null, "   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorNameIsNotGiven_ReturnsNotNull(string creatorName, string creatorMailAddress)
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), creatorName, creatorMailAddress);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(" ", null)]
        [TestCase("  ", null)]
        [TestCase("   ", null)]
        [TestCase(null, "")]
        [TestCase(null, " ")]
        [TestCase(null, "  ")]
        [TestCase(null, "   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorNameIsNotGiven_ReturnsMarkdownParagraph(string creatorName, string creatorMailAddress)
        {
            IDomainObjectToMarkdownConverter<object> sut = CreateSut();

            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(GetCreationTime(), creatorName, creatorMailAddress);

            Assert.That(result, Is.TypeOf<MarkdownParagraph>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(" ", null)]
        [TestCase("  ", null)]
        [TestCase("   ", null)]
        [TestCase(null, "")]
        [TestCase(null, " ")]
        [TestCase(null, "  ")]
        [TestCase(null, "   ")]
        public void GetCreatorInformationMarkdown_WhenOnlyCreatorNameIsNotGiven_ReturnsMarkdownParagraphForCreatorInformation(string creatorName, string creatorMailAddress)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            IDomainObjectToMarkdownConverter<object> sut = CreateSut(formatProvider);

            DateTime creationTime = GetCreationTime();
            IMarkdownBlockElement result = ((MyDomainObjectToMarkdownConverter)sut).GetCreatorInformationMarkdown(creationTime, creatorName, creatorMailAddress);

            Assert.That(result.ToString(), Is.EqualTo($"*Udskrevet den {creationTime.ToString("d", formatProvider)} kl. {creationTime.ToString("t", formatProvider)}*{Environment.NewLine}"));
        }

        private IDomainObjectToMarkdownConverter<object> CreateSut(IFormatProvider formatProvider = null)
        {
            return new MyDomainObjectToMarkdownConverter(formatProvider ?? CultureInfo.InvariantCulture);
        }

        private DateTime GetCreationTime()
        {
            return DateTime.Now.AddMinutes(_random.Next(0, 120) * -1);
        }

        private string GetCreatorName()
        {
            return _fixture.Create<string>();
        }

        private string GetCreatorMailAddress()
        {
            return $"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local";
        }

        private class MyDomainObjectToMarkdownConverter : DomainObjectToMarkdownConverterBase<object>
        {
            #region Constructors

            public MyDomainObjectToMarkdownConverter(IFormatProvider formatProvider)
                : base(formatProvider)
            {
            }

            #endregion

            #region Methods

            protected override Task WriteDocumentAsync(object domainObject, IMarkdownDocument markdownDocument) => throw new NotSupportedException();

            public new IMarkdownBlockElement GetCreatorInformationMarkdown(DateTime creationTime, string creatorName, string creatorMailAddress)
            {
                return base.GetCreatorInformationMarkdown(creationTime, creatorName, creatorMailAddress);
            }

            #endregion
        }
    }
}