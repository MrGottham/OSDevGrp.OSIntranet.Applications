using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public enum PostingWarningReason
    {
        [Display(Name = "Konto er overtrukket")]
        AccountIsOverdrawn = 0,

        [Display(Name = "Budgetkonto har endnu ikke nået det budgetterede beløb")]
        ExpectedIncomeHasNotBeenReachedYet = 1,

        [Display(Name = "Budgetkonto har overskredet det budgetterede beløb")]
        ExpectedExpensesHaveAlreadyBeenReached = 2
    }
}