using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountViewModel : AccountCoreDataViewModel
    {
        [Display(Name = "Kontogruppe", ShortName = "Gruppe", Description = "Kontogruppe")]
        [Required(ErrorMessage = "Der skal vælges en kontogruppe.")]
        public AccountGroupViewModel AccountGroup { get; set; }

        [Display(Name = "Kreditoplysninger pr. dags dato", ShortName = "Kreditopl. pr. dags dato", Description = "Kreditoplysninger pr. dags dato")]
        public CreditInfoValuesViewModel ValuesAtStatusDate { get; set; }

        [Display(Name = "Kreditoplysninger ved sidste måneds afslutning", ShortName = "Kreditopl. ved sidste måneds afslutning", Description = "Kreditoplysninger ved sidste måneds afslutning")]
        public CreditInfoValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Display(Name = "Kreditoplysninger ved sidste års afslutning", ShortName = "Kreditopl. ved sidste års afslutning", Description = "Kreditoplysninger ved sidste års afslutning")]
        public CreditInfoValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }

        public CreditInfoDictionaryViewModel CreditInfos { get; set; }

        public IReadOnlyCollection<AccountGroupViewModel> AccountGroups { get; set; }
    }

    public static class AccountViewModelExtensions
    {
        public static string GetAction(this AccountViewModel accountViewModel)
        {
            NullGuard.NotNull(accountViewModel, nameof(accountViewModel));

            return accountViewModel.EditMode == EditMode.Create ? "CreateAccount" : "UpdateAccount";
        }
    }
}