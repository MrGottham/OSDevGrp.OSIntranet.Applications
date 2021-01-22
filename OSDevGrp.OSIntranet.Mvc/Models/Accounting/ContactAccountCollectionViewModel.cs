using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ContactAccountCollectionViewModel : AccountCollectionViewModelBase<ContactAccountViewModel>
    {
        [Display(Name = "Saldi pr. dags dato", ShortName = "Saldi pr. dags dato", Description = "Saldi pr. dags dato")]
        public ContactAccountCollectionValuesViewModel ValuesAtStatusDate { get; set; }

        [Display(Name = "Saldi ved sidste måneds afslutning", ShortName = "Saldi ved sidste måneds afslutning", Description = "Saldi ved sidste måneds afslutning")]
        public ContactAccountCollectionValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Display(Name = "Saldi ved sidste års afslutning", ShortName = "Saldi ved sidste års afslutning", Description = "Saldi ved sidste års afslutning")]
        public ContactAccountCollectionValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }
    }
}