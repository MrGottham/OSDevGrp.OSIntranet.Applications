using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountingCommandBase
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountingCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));
            
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetLetterHeadAsyncWasCalledOnCommonRepository()
        {
            int letterHeadNumber = _fixture.Create<int>();
            IAccountingCommand sut = CreateSut(letterHeadNumber: letterHeadNumber);

            sut.ToDomain(_commonRepositoryMock.Object);

            _commonRepositoryMock.Verify(m => m.GetLetterHeadAsync(It.Is<int>(value => value == letterHeadNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccounting()
        {
            IAccountingCommand sut = CreateSut();

            IAccounting result = sut.ToDomain(_commonRepositoryMock.Object);

            Assert.That(result, Is.TypeOf<Domain.Accounting.Accounting>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountingWithAccountingNumberFromCommand()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccountingCommand sut = CreateSut(accountingNumber);

            int result = sut.ToDomain(_commonRepositoryMock.Object).Number;

            Assert.That(result, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountingWithNameFromCommand()
        {
            string name = _fixture.Create<string>();
            IAccountingCommand sut = CreateSut(name: name);

            string result = sut.ToDomain(_commonRepositoryMock.Object).Name;

            Assert.That(result, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountingWithLetterHeadFromCommonRepository()
        {
            ILetterHead letterHead = _fixture.BuildLetterHeadMock().Object;
            IAccountingCommand sut = CreateSut(letterHead: letterHead);

            ILetterHead result = sut.ToDomain(_commonRepositoryMock.Object).LetterHead;

            Assert.That(result, Is.EqualTo(letterHead));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountingWithBalanceBelowZeroFromCommand()
        {
            BalanceBelowZeroType balanceBelowZero = _fixture.Create<BalanceBelowZeroType>();
            IAccountingCommand sut = CreateSut(balanceBelowZero: balanceBelowZero);

            BalanceBelowZeroType result = sut.ToDomain(_commonRepositoryMock.Object).BalanceBelowZero;

            Assert.That(result, Is.EqualTo(balanceBelowZero));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountingWithBackDatingFromCommand()
        {
            int backDating = _fixture.Create<int>();
            IAccountingCommand sut = CreateSut(backDating: backDating);

            int result = sut.ToDomain(_commonRepositoryMock.Object).BackDating;

            Assert.That(result, Is.EqualTo(backDating));
        }

        private IAccountingCommand CreateSut(int? accountingNumber = null, string name = null, int? letterHeadNumber = null, ILetterHead letterHead = null, BalanceBelowZeroType? balanceBelowZero = null, int? backDating = null)
        {
            _commonRepositoryMock.Setup(m => m.GetLetterHeadAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => letterHead ?? _fixture.BuildLetterHeadMock().Object));

            return _fixture.Build<Sut>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .With(m => m.LetterHeadNumber, letterHeadNumber ?? _fixture.Create<int>())
                .With(m => m.BalanceBelowZero, balanceBelowZero ?? _fixture.Create<BalanceBelowZeroType>())
                .With(m => m.BackDating, backDating ?? _fixture.Create<int>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.AccountingCommandBase
        {
        }
    }
}