using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountCollectionViewModel : AccountCollectionViewModelBase<AccountViewModel>
    {
        [Display(Name = "Balance pr. dags dato", ShortName = "Balance pr. dags dato", Description = "Balance pr. dags dato")]
        public AccountCollectionValuesViewModel ValuesAtStatusDate { get; set; }

        [Display(Name = "Balance ved sidste måneds afslutning", ShortName = "Balance ved sidste måneds afslutning", Description = "Balance ved sidste måneds afslutning")]
        public AccountCollectionValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Display(Name = "Balance ved sidste års afslutning", ShortName = "Balance ved sidste års afslutning", Description = "Balance ved sidste års afslutning")]
        public AccountCollectionValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }
    }
}