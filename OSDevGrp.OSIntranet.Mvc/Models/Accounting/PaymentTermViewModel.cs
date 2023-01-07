using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PaymentTermViewModel : AuditableViewModelBase
    {
        #region Properties

        [Display(Name = "Nummer", ShortName = "Nummer", Description = "Nummer")]
        [Required(ErrorMessage = "Nummeret skal udfyldes.")]
        [Range(1, 99, ErrorMessage = "Nummeret skal være mellem {1} og {2}.")]
        public int Number { get; set; }

        [Display(Name = "Navn", ShortName = "Navn", Description = "Navn")]
        [Required(ErrorMessage = "Navnet skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på navnet skal være mellem {2} og {1} tegn.")]
        public string Name { get; set; }

        public bool IsProtected { get; set; }

        public bool Deletable { get; set; }

        #endregion
    }

    public static class PaymentTermViewModelExtensions
    {
        public static string GetDeletionLink(this PaymentTermViewModel paymentTermViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(paymentTermViewModel, nameof(paymentTermViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeletePaymentTerm", "Accounting");
        }

        public static string GetDeletionData(this PaymentTermViewModel paymentTermViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(paymentTermViewModel, nameof(paymentTermViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"number: '{paymentTermViewModel.Number}', {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        public static bool IsKnownPaymentTerm(this PaymentTermViewModel paymentTermViewModel, IEnumerable<PaymentTermViewModel> knownPaymentTermViewModels)
        {
            NullGuard.NotNull(paymentTermViewModel, nameof(paymentTermViewModel))
                .NotNull(knownPaymentTermViewModels, nameof(knownPaymentTermViewModels));

            return knownPaymentTermViewModels.Any(knownPaymentTermViewModel => knownPaymentTermViewModel.Number == paymentTermViewModel.Number);
        }

        public static SelectListItem SelectListItemFor(this PaymentTermViewModel paymentTermViewModel, bool selected)
        {
            NullGuard.NotNull(paymentTermViewModel, nameof(paymentTermViewModel));

            return new SelectListItem(paymentTermViewModel.Name, Convert.ToString(paymentTermViewModel.Number), selected);
        }

        public static IEnumerable<SelectListItem> SelectListFor(this IEnumerable<PaymentTermViewModel> paymentTermViewModels, int? selectedValue)
        {
            NullGuard.NotNull(paymentTermViewModels, nameof(paymentTermViewModels));

            return paymentTermViewModels.Select(paymentTermViewModel => paymentTermViewModel.SelectListItemFor(selectedValue.HasValue && selectedValue.Value == paymentTermViewModel.Number)).ToArray();
        }
    }
}