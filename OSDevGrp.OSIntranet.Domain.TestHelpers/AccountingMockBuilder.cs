using System;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class AccountingMockBuilder
    {
        public static Mock<IAccounting> BuildAccountingMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccounting> accountingMock = new Mock<IAccounting>();
            accountingMock.Setup(m => m.Number)
                .Returns(fixture.Create<int>());
            accountingMock.Setup(m => m.Name)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.LetterHead)
                .Returns(fixture.BuildLetterHeadMock().Object);
            accountingMock.Setup(m => m.BalanceBelowZero)
                .Returns(fixture.Create<BalanceBelowZeroType>());
            accountingMock.Setup(m => m.BackDating)
                .Returns(fixture.Create<int>());
            accountingMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            accountingMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountingMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountingMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return accountingMock;
        }

        public static Mock<IAccountGroup> BuildAccountGroupMock(this Fixture fixture, int? number = null, string name = null, AccountGroupType? accountGroupType = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccountGroup> accountGroupMock = new Mock<IAccountGroup>();
            accountGroupMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            accountGroupMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            accountGroupMock.Setup(m => m.AccountGroupType)
                .Returns(accountGroupType ?? fixture.Create<AccountGroupType>());
            accountGroupMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            accountGroupMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountGroupMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            accountGroupMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountGroupMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return accountGroupMock;
        }

        public static Mock<IBudgetAccountGroup> BuildBudgetAccountGroupMock(this Fixture fixture, int? number = null, string name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetAccountGroup> budgetAccountGroupMock = new Mock<IBudgetAccountGroup>();
            budgetAccountGroupMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            budgetAccountGroupMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            budgetAccountGroupMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            budgetAccountGroupMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountGroupMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetAccountGroupMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountGroupMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return budgetAccountGroupMock;
        }

        public static Mock<IPaymentTerm> BuildPaymentTermMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPaymentTerm> paymentTermMock = new Mock<IPaymentTerm>();
            paymentTermMock.Setup(m => m.Number)
                .Returns(fixture.Create<int>());
            paymentTermMock.Setup(m => m.Name)
                .Returns(fixture.Create<string>());
            return paymentTermMock;
        }
    }
}