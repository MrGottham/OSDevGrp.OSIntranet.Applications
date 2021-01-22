using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ContactAccountCollectionViewModel : AccountCollectionViewModelBase<ContactAccountViewModel>
    {
        [Display(Name = "Saldi pr. dags dato", ShortName = "Saldi pr. dags dato", Description = "Saldi pr. dags dato")]
        public ContactAccountCollectionValuesViewModel ValuesAtStatusDate { get; set; }

        [Display(Name = "Saldi ved sidste m�neds afslutning", ShortName = "Saldi ved sidste m�neds afslutning", Description = "Saldi ved sidste m�neds afslutning")]
        public ContactAccountCollectionValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Display(Name = "Saldi ved sidste �rs afslutning", ShortName = "Saldi ved sidste �rs afslutning", Description = "Saldi ved sidste �rs afslutning")]
        public ContactAccountCollectionValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }
    }
}