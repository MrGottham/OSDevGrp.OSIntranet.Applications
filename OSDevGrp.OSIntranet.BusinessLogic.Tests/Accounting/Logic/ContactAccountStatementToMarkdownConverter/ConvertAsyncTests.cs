using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.ContactAccountStatementToMarkdownConverter
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
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("domainObject"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountNameWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.AccountName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertAccountNumberWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertMailAddressWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.MailAddress, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPrimaryPhoneWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.PrimaryPhone, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertSecondaryPhoneWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.SecondaryPhone, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPaymentTermWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.PaymentTerm, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertNameWasCalledOnPaymentTermFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IPaymentTerm> paymentTermMock = _fixture.BuildPaymentTermMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(paymentTerm: paymentTermMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            paymentTermMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesAtStatusDateWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtStatusDateFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactInfoValues> contactInfoValuesMock = _fixture.BuildContactInfoValuesMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(valuesAtStatusDate: contactInfoValuesMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            contactInfoValuesMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtEndOfLastMonthFromStatusDateFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactInfoValues> contactInfoValuesMock = _fixture.BuildContactInfoValuesMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(valuesAtEndOfLastMonthFromStatusDate: contactInfoValuesMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            contactInfoValuesMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBalanceWasCalledOnValuesAtEndOfLastYearFromStatusDateFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactInfoValues> contactInfoValuesMock = _fixture.BuildContactInfoValuesMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(valuesAtEndOfLastYearFromStatusDate: contactInfoValuesMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            contactInfoValuesMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertPostingLineCollectionWasCalledOnContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertOrderedWasCalledOnPostingLineCollectionFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            postingLineCollectionMock.Verify(m => m.Ordered(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertTopWasCalledOnOrderedPostingLineCollectionFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IPostingLineCollection> orderedPostingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollectionMock.Object).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(contactAccount);

            orderedPostingLineCollectionMock.Verify(m => m.Top(It.Is<int>(value => value == 30)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertContactAccountValuesAtPostingDateWasCalledForEachPostingLineInTopOrderedPostingLineCollectionFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

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
            IPostingLineCollection topPostingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray()).Object;
            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(topPostingLineCollection: topPostingLineCollection).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(contactAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ContactAccountValuesAtPostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_AssertBalanceWasCalledOnContactAccountValuesAtPostingDateForEachPostingLineInTopOrderedPostingLineCollectionFromContactAccount()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            Mock<IContactInfoValues>[] contactInfoValuesMockCollection =
            {
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock()
            };
            IPostingLineCollection topPostingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: contactInfoValuesMockCollection.Select(contactInfoValuesMock => _fixture.BuildPostingLineMock(contactAccount: _fixture.BuildContactAccountMock().Object, contactAccountValuesAtPostingDate: contactInfoValuesMock.Object).Object).ToArray()).Object;
            IPostingLineCollection orderedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(topPostingLineCollection: topPostingLineCollection).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(orderedPostingLineCollection: orderedPostingLineCollection).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            await sut.ConvertAsync(contactAccount);

            foreach (Mock<IContactInfoValues> contactInfoValuesMock in contactInfoValuesMockCollection)
            {
                contactInfoValuesMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotNull()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenCalled_ReturnNotEmpty()
        {
            IContactAccountStatementToMarkdownConverter sut = CreateSut();

            string result = await sut.ConvertAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        private IContactAccountStatementToMarkdownConverter CreateSut(DateTime? statusDate = null)
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(statusDate?.Date ?? DateTime.Today.AddDays(_random.Next(0, 365) * -1));

            _claimResolverMock.Setup(m => m.GetName())
                .Returns(_fixture.Create<string>());
            _claimResolverMock.Setup(m => m.GetMailAddress())
                .Returns($"{_fixture.Create<string>().Replace("@", string.Empty)}@{_fixture.Create<string>().Replace("@", string.Empty)}.local");

            return new BusinessLogic.Accounting.Logic.ContactAccountStatementToMarkdownConverter(_statusDateProviderMock.Object, _claimResolverMock.Object);
        }
    }
}