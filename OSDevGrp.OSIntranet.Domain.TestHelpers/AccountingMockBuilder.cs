using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class AccountingMockBuilder
    {
        public static Mock<IAccountGroup> BuildAccountGroupMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccountGroup> accountGroupMock = new Mock<IAccountGroup>();
            accountGroupMock.Setup(m => m.Number)
                .Returns(fixture.Create<int>());
            accountGroupMock.Setup(m => m.Name)
                .Returns(fixture.Create<string>());
            accountGroupMock.Setup(m => m.AccountGroupType)
                .Returns(fixture.Create<AccountGroupType>());
            return accountGroupMock;
        }

        public static Mock<IBudgetAccountGroup> BuildBudgetAccountGroupMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetAccountGroup> budgetAccountGroupMock = new Mock<IBudgetAccountGroup>();
            budgetAccountGroupMock.Setup(m => m.Number)
                .Returns(fixture.Create<int>());
            budgetAccountGroupMock.Setup(m => m.Name)
                .Returns(fixture.Create<string>());
            return budgetAccountGroupMock;
        }
    }
}