using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public enum PostingWarningReason
    {
        [EnumMember(Value = "AccountIsOverdrawn")]
        AccountIsOverdrawn = 0,

        [EnumMember(Value = "ExpectedIncomeHasNotBeenReachedYet")]
        ExpectedIncomeHasNotBeenReachedYet = 1,

        [EnumMember(Value = "ExpectedExpensesHaveAlreadyBeenReached")]
        ExpectedExpensesHaveAlreadyBeenReached = 2
    }
}