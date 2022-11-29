using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Markdown;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.AccountToMarkdownConverterBase
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
        public void ConvertAsync_WhenAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountingWasCalledOnAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            Mock<IAccount> accountMock = _fixture.BuildAccountMock(accounting, isEmpty: true);
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.Accounting, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostingLineCollectionWasCalledOnAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertOrderedWasCalledOnPostingLineCollectionFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            await sut.ConvertAsync(account);

            postingLineCollectionMock.Verify(m => m.Ordered(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostingDateWasCalledForEachPostingLineInOrderedPostingLineCollectionFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IPostingLine>[] postingLineMockCollection =
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray()).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(account);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertDetailsWasCalledForEachPostingLineInOrderedPostingLineCollectionFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IPostingLine>[] postingLineMockCollection =
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray()).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(account);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.Details, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostingValueWasCalledForEachPostingLineInOrderedPostingLineCollectionFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IPostingLine>[] postingLineMockCollection =
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray()).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(account);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountValuesAtPostingDateWasCalledForEachPostingLineInOrderedPostingLineCollectionFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IPostingLine>[] postingLineMockCollection =
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray()).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(account);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.AccountValuesAtPostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBalanceWasCalledOnAccountValuesAtPostingDateForEachPostingLineInOrderedPostingLineCollectionFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ICreditInfoValues>[] creditInfoValuesMockCollection =
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: creditInfoValuesMockCollection.Select(creditInfoValuesMock => _fixture.BuildPostingLineMock(accountValuesAtPostingDate: creditInfoValuesMock.Object).Object).ToArray()).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(account);

            foreach (Mock<ICreditInfoValues> creditInfoValuesMock in creditInfoValuesMockCollection)
            {
                creditInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertLetterHeadWasCalledOnAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            await sut.ConvertAsync(account);

            accountingMock.Verify(m => m.LetterHead, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNullOnAccounting_AssertNameWasCalledOnAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock(hasLetterHead: false);
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            await sut.ConvertAsync(account);

            accountingMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNotNullOnAccounting_AssertLine1WasCalledOnLetterHeadFromAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ILetterHead> letterHeadMock = _fixture.BuildLetterHeadMock();
            IAccounting accounting = _fixture.BuildAccountingMock(letterHead: letterHeadMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            letterHeadMock.Verify(m => m.Line1, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNotNullOnAccounting_AssertLine2WasCalledOnLetterHeadFromAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ILetterHead> letterHeadMock = _fixture.BuildLetterHeadMock();
            IAccounting accounting = _fixture.BuildAccountingMock(letterHead: letterHeadMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            letterHeadMock.Verify(m => m.Line2, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNotNullOnAccounting_AssertLine3WasCalledOnLetterHeadFromAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ILetterHead> letterHeadMock = _fixture.BuildLetterHeadMock();
            IAccounting accounting = _fixture.BuildAccountingMock(letterHead: letterHeadMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            letterHeadMock.Verify(m => m.Line3, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNotNullOnAccounting_AssertLine4WasCalledOnLetterHeadFromAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ILetterHead> letterHeadMock = _fixture.BuildLetterHeadMock();
            IAccounting accounting = _fixture.BuildAccountingMock(letterHead: letterHeadMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            letterHeadMock.Verify(m => m.Line4, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNotNullOnAccounting_AssertLine5WasCalledOnLetterHeadFromAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ILetterHead> letterHeadMock = _fixture.BuildLetterHeadMock();
            IAccounting accounting = _fixture.BuildAccountingMock(letterHead: letterHeadMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            letterHeadMock.Verify(m => m.Line5, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNotNullOnAccounting_AssertLine6WasCalledOnLetterHeadFromAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ILetterHead> letterHeadMock = _fixture.BuildLetterHeadMock();
            IAccounting accounting = _fixture.BuildAccountingMock(letterHead: letterHeadMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            letterHeadMock.Verify(m => m.Line6, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenLetterHeadIsNotNullOnAccounting_AssertLine7WasCalledOnLetterHeadFromAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            Mock<ILetterHead> letterHeadMock = _fixture.BuildLetterHeadMock();
            IAccounting accounting = _fixture.BuildAccountingMock(letterHead: letterHeadMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            letterHeadMock.Verify(m => m.Line7, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetStatusDateWasCalledOnStatusDateProvider()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            _statusDateProviderMock.Verify(m => m.GetStatusDate(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetNameWasCalledOnClaimResolver()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            _claimResolverMock.Verify(m => m.GetName(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetMailAddressWasCalledOnClaimResolver()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            _claimResolverMock.Verify(m => m.GetMailAddress(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetHeaderMarkdownCollectionWasCalledOnAccountToMarkdownConverterBase()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetHeaderMarkdownCollectionWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetHeaderMarkdownCollectionWasCalledOnAccountToMarkdownConverterBaseWithAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetHeaderMarkdownCollectionCalledWithAccounting, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetHeaderMarkdownCollectionWasCalledOnAccountToMarkdownConverterBaseWithAccountFromArgument()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            await sut.ConvertAsync(account);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetHeaderMarkdownCollectionCalledWithAccount, Is.EqualTo(account));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetHeaderMarkdownCollectionWasCalledOnAccountToMarkdownConverterBaseWithStatusDateFromStatusDateProvider()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IAccountToMarkdownConverter<IAccount> sut = CreateSut(statusDate);

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetHeaderMarkdownCollectionCalledWithStatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentHeaderMarkdownWasCalledOnAccountToMarkdownConverterBase()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentHeaderMarkdownWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentHeaderMarkdownWasCalledOnAccountToMarkdownConverterBaseWithAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentHeaderMarkdownCalledWithAccounting, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentHeaderMarkdownWasCalledOnAccountToMarkdownConverterBaseWithAccountFromArgument()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            await sut.ConvertAsync(account);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentHeaderMarkdownCalledWithAccount, Is.EqualTo(account));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentHeaderMarkdownWasCalledOnAccountToMarkdownConverterBaseWithStatusDateFromStatusDateProvider()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IAccountToMarkdownConverter<IAccount> sut = CreateSut(statusDate);

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentHeaderMarkdownCalledWithStatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentExplanationMarkdownCollectionWasCalledOnAccountToMarkdownConverterBase()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentExplanationMarkdownCollectionWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentExplanationMarkdownCollectionWasCalledOnAccountToMarkdownConverterBaseWithAccountingFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            await sut.ConvertAsync(account);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentExplanationMarkdownCollectionCalledWithAccounting, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentExplanationMarkdownCollectionWasCalledOnAccountToMarkdownConverterBaseWithAccountFromArgument()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            await sut.ConvertAsync(account);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentExplanationMarkdownCollectionCalledWithAccount, Is.EqualTo(account));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetContentExplanationMarkdownCollectionWasCalledOnAccountToMarkdownConverterBaseWithStatusDateFromStatusDateProvider()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IAccountToMarkdownConverter<IAccount> sut = CreateSut(statusDate);

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetContentExplanationMarkdownCollectionCalledWithStatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetPostingLineCollectionForContentWasCalledOnAccountToMarkdownConverterBase()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetPostingLineCollectionForContentWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetPostingLineCollectionForContentWasCalledOnAccountToMarkdownConverterBaseWithOrderedPostingLineCollectionFromAccount()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock().Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(account);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetPostingLineCollectionForContentCalledWithPostingLineCollection, Is.EqualTo(orderedPostingLineCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertGetPostingLineCollectionForContentWasCalledOnAccountToMarkdownConverterBaseWithStatusDateFromStatusDateProvider()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
            IAccountToMarkdownConverter<IAccount> sut = CreateSut(statusDate);

            await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(((MyAccountToMarkdownConverterBase)sut).GetPostingLineCollectionForContentCalledWithStatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotNull()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotEmpty()
        {
            IAccountToMarkdownConverter<IAccount> sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        private IAccountToMarkdownConverter<IAccount> CreateSut(DateTime? statusDate = null)
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(statusDate?.Date ?? DateTime.Today.AddDays(_random.Next(0, 365) * -1));

            _claimResolverMock.Setup(m => m.GetName())
                .Returns(_fixture.Create<string>());
            _claimResolverMock.Setup(m => m.GetMailAddress())
                .Returns($"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local");

            return new MyAccountToMarkdownConverterBase(_statusDateProviderMock.Object, _claimResolverMock.Object, CultureInfo.InvariantCulture);
        }

        private class MyAccountToMarkdownConverterBase : BusinessLogic.Accounting.Logic.AccountToMarkdownConverterBase<IAccount>
        {
            #region Constructor

            public MyAccountToMarkdownConverterBase(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver, IFormatProvider formatProvider) 
                : base(statusDateProvider, claimResolver, formatProvider)
            {
            }

            #endregion

            #region Properties

            public bool GetHeaderMarkdownCollectionWasCalled { get; private set; }

            public IAccounting GetHeaderMarkdownCollectionCalledWithAccounting { get; private set; }

            public IAccount GetHeaderMarkdownCollectionCalledWithAccount { get; private set; }

            public DateTime GetHeaderMarkdownCollectionCalledWithStatusDate { get; private set; }

            public bool GetContentHeaderMarkdownWasCalled { get; private set; }

            public IAccounting GetContentHeaderMarkdownCalledWithAccounting { get; private set; }

            public IAccount GetContentHeaderMarkdownCalledWithAccount { get; private set; }

            public DateTime GetContentHeaderMarkdownCalledWithStatusDate { get; private set; }

            public bool GetContentExplanationMarkdownCollectionWasCalled { get; private set; }

            public IAccounting GetContentExplanationMarkdownCollectionCalledWithAccounting { get; private set; }

            public IAccount GetContentExplanationMarkdownCollectionCalledWithAccount { get; private set; }

            public DateTime GetContentExplanationMarkdownCollectionCalledWithStatusDate { get; private set; }

            public bool GetPostingLineCollectionForContentWasCalled { get; private set; }

            public IPostingLineCollection GetPostingLineCollectionForContentCalledWithPostingLineCollection { get; private set; }

            public DateTime GetPostingLineCollectionForContentCalledWithStatusDate { get; private set; }

            #endregion

            #region Methods

            protected override IEnumerable<IMarkdownBlockElement> GetHeaderMarkdownCollection(IAccounting accounting, IAccount account, DateTime statusDate)
            {
                NullGuard.NotNull(accounting, nameof(accounting))
                    .NotNull(account, nameof(account));

                GetHeaderMarkdownCollectionWasCalled = true;
                GetHeaderMarkdownCollectionCalledWithAccounting = accounting;
                GetHeaderMarkdownCollectionCalledWithAccount = account;
                GetHeaderMarkdownCollectionCalledWithStatusDate = statusDate;

                return base.GetHeaderMarkdownCollection(accounting, account, statusDate);
            }

            protected override IMarkdownBlockElement GetContentHeaderMarkdown(IAccounting accounting, IAccount account, DateTime statusDate)
            {
                NullGuard.NotNull(accounting, nameof(accounting))
                    .NotNull(account, nameof(account));

                GetContentHeaderMarkdownWasCalled = true;
                GetContentHeaderMarkdownCalledWithAccounting = accounting;
                GetContentHeaderMarkdownCalledWithAccount = account;
                GetContentHeaderMarkdownCalledWithStatusDate = statusDate;

                return base.GetContentHeaderMarkdown(accounting, account, statusDate);
            }

            protected override IEnumerable<IMarkdownBlockElement> GetContentExplanationMarkdownCollection(IAccounting accounting, IAccount account, DateTime statusDate)
            {
                NullGuard.NotNull(accounting, nameof(accounting))
                    .NotNull(account, nameof(account));

                GetContentExplanationMarkdownCollectionWasCalled = true;
                GetContentExplanationMarkdownCollectionCalledWithAccounting = accounting;
                GetContentExplanationMarkdownCollectionCalledWithAccount = account;
                GetContentExplanationMarkdownCollectionCalledWithStatusDate = statusDate;

                return base.GetContentExplanationMarkdownCollection(accounting, account, statusDate);
            }

            protected override IEnumerable<IPostingLine> GetPostingLineCollectionForContent(IPostingLineCollection postingLineCollection, DateTime statusDate)
            {
                NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

                GetPostingLineCollectionForContentWasCalled = true;
                GetPostingLineCollectionForContentCalledWithPostingLineCollection = postingLineCollection;
                GetPostingLineCollectionForContentCalledWithStatusDate = statusDate;

                return base.GetPostingLineCollectionForContent(postingLineCollection, statusDate);
            }

            protected override decimal GetBalance(IPostingLine postingLine) => postingLine.AccountValuesAtPostingDate?.Balance ?? 0M;

            #endregion
        }
    }
}