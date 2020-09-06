using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    public class HomeOperationsViewModel
    {
        public bool CanAccessContacts { get; set; }

        public bool HasAcquiredMicrosoftGraphToken { get; set; }

        public int UpcomingBirthdaysWithinDays { get; set; }

        public bool CanAccessAccountings { get; set; }

        public int? AccountingNumber { get; set; }
    }

    public static class HomeOperationsViewModelExtensions
    {
        public static string GetUpcomingBirthdaysUrl(this HomeOperationsViewModel homeOperationsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(homeOperationsViewModel, nameof(homeOperationsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("UpcomingBirthdays", "Home", new {withinDays = homeOperationsViewModel.UpcomingBirthdaysWithinDays});
        }

        public static string GetAccountingInformationUrl(this HomeOperationsViewModel homeOperationsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(homeOperationsViewModel, nameof(homeOperationsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("AccountingInformation", "Home", new {accountingNumber = homeOperationsViewModel.AccountingNumber ?? 0});
        }
    }
}