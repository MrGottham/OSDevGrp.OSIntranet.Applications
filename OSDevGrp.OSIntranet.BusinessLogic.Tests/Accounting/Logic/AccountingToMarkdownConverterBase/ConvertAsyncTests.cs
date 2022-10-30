using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoFixture;
using Markdown;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.AccountingToMarkdownConverterBase
{
    [TestFixture]
    public class ConvertAsyncTests
    {
        #region Private variables

        private Mock<IStatusDateProvider> _statusDateProviderMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _statusDateProviderMock = new Mock<IStatusDateProvider>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ConvertAsync_WhenAccountingIsNull_ThrowsArgumentNullException()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetStatusDateWasCalledOnStatusDateProvider()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            _statusDateProviderMock.Verify(m => m.GetStatusDate(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetNameWasCalledOnClaimResolver()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            _claimResolverMock.Verify(m => m.GetName(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetMailAddressWasCalledOnClaimResolver()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            _claimResolverMock.Verify(m => m.GetMailAddress(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertNameWasCalledOnAccounting()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            await sut.ConvertAsync(accountingMock.Object);

            accountingMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetHeaderWasCalledOnAccountingToMarkdownConverterBase()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(((MyAccountingToMarkdownConverter)sut).GetHeaderWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetHeaderWasCalledOnAccountingToMarkdownConverterBaseWithStatusDate()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IAccountingToMarkdownConverter sut = CreateSut(statusDate);

            await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(((MyAccountingToMarkdownConverter)sut).GetHeaderWasCalledWithStatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertWriteContentAsyncWasCalledOnAccountingToMarkdownConverterBase()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(((MyAccountingToMarkdownConverter)sut).WriteContentAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertWriteContentAsyncWasCalledOnAccountingToMarkdownConverterBaseWithAccounting()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IAccountingToMarkdownConverter sut = CreateSut(statusDate);

            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            await sut.ConvertAsync(accounting);

            Assert.That(((MyAccountingToMarkdownConverter)sut).WriteContentAsyncWasCalledWithAccounting, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertWriteContentAsyncWasCalledOnAccountingToMarkdownConverterBaseWithStatusDate()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IAccountingToMarkdownConverter sut = CreateSut(statusDate);

            await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(((MyAccountingToMarkdownConverter)sut).WriteContentAsyncWasCalledWithStatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotNull()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotEmpty()
        {
            IAccountingToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        private IAccountingToMarkdownConverter CreateSut(DateTime? statusDate = null)
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(statusDate?.Date ?? DateTime.Today.AddDays(_random.Next(0, 365) * -1));

            _claimResolverMock.Setup(m => m.GetName())
                .Returns(_fixture.Create<string>());
            _claimResolverMock.Setup(m => m.GetMailAddress())
                .Returns($"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local");

            return new MyAccountingToMarkdownConverter(_fixture.Create<string>(), _statusDateProviderMock.Object, _claimResolverMock.Object, CultureInfo.InvariantCulture);
        }

        private class MyAccountingToMarkdownConverter : BusinessLogic.Accounting.Logic.AccountingToMarkdownConverterBase
        {
            #region Private variables

            private readonly string _header;

            #endregion

            #region Constructor

            public MyAccountingToMarkdownConverter(string header, IStatusDateProvider statusDateProvider, IClaimResolver claimResolver, IFormatProvider formatProvider)
                : base(statusDateProvider, claimResolver, formatProvider)
            {
                NullGuard.NotNullOrWhiteSpace(header, nameof(header));

                _header = header;
            }

            #endregion

            #region Properties

            public bool GetHeaderWasCalled { get; private set; }

            public DateTime GetHeaderWasCalledWithStatusDate { get; private set; }

            public bool WriteContentAsyncWasCalled { get; private set; }

            public IAccounting WriteContentAsyncWasCalledWithAccounting { get; private set; }

            public DateTime WriteContentAsyncWasCalledWithStatusDate { get; private set; }

            #endregion

            #region Methods

            protected override string GetHeader(DateTime statusDate)
            {
                GetHeaderWasCalled = true;
                GetHeaderWasCalledWithStatusDate = statusDate;

                return $"{_header} pr. {statusDate.ToString("D", FormatProvider)}";
            }

            protected override Task WriteContentAsync(IAccounting accounting, DateTime statusDate, IMarkdownDocument markdownDocument)
            {
                NullGuard.NotNull(accounting, nameof(accounting))
                    .NotNull(markdownDocument, nameof(markdownDocument));

                WriteContentAsyncWasCalled = true;
                WriteContentAsyncWasCalledWithAccounting = accounting;
                WriteContentAsyncWasCalledWithStatusDate = statusDate;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}