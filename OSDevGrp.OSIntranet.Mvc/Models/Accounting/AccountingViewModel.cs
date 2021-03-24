using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Common;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountingViewModel : AccountingIdentificationViewModel
    {
        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved, som regnskabet skal benytte")]
        [Required(ErrorMessage = "Der skal vælges et brevhoved, som regnskabet skal benytte.")]
        public LetterHeadViewModel LetterHead { get; set; }

        [Display(Name = "Saldo under kr. 0,00", ShortName = "Saldo under kr. 0,00", Description = "Angivelse af, om der er tale om en debitor eller kreditor, når saldoen er under kr. 0,00")]
        [Required(ErrorMessage = "Angivelsen af, om der er tale om en debitor eller kreditor, når saldoen er under kr. 0,00, skal vælges.")]
        public BalanceBelowZeroType BalanceBelowZero { get; set; }

        [Display(Name = "Antal dage for tilbagedatering", ShortName = "Tilbagedatering", Description = "Antal dage, der kan tilbagedateres")]
        [Required(ErrorMessage = "Antal dage for tilbagedatering skal udfyldes.")]
        [Range(0, 365, ErrorMessage = "Antallet af dage for tilbagedatering skal være mellem {1} og {2} dage.")]
        public int BackDating { get; set; }

        public bool Deletable { get; set; }

        public AccountDictionaryViewModel Accounts { get; set; }

        public BudgetAccountDictionaryViewModel BudgetAccounts { get; set; }

        public ContactAccountCollectionViewModel ContactAccounts { get; set; }

        public IReadOnlyCollection<LetterHeadViewModel> LetterHeads { get; set; }
    }
}