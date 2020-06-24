using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class ContactGroupViewModel : AuditableViewModelBase
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

        public bool Deletable { get; set; }

        #endregion
    }

    public static class ContactGroupViewModelExtensions
    {
        public static string GetDeletionLink(this ContactGroupViewModel contactGroupViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactGroupViewModel, nameof(contactGroupViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteContactGroup", "Contact");
        }

        public static string GetDeletionData(this ContactGroupViewModel contactGroupViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(contactGroupViewModel, nameof(contactGroupViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"number: '{contactGroupViewModel.Number}', {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        public static bool IsKnownContactGroup(this ContactGroupViewModel contactGroupViewModel, IEnumerable<ContactGroupViewModel> knownContactGroupViewModels)
        {
            NullGuard.NotNull(contactGroupViewModel, nameof(contactGroupViewModel))
                .NotNull(knownContactGroupViewModels, nameof(knownContactGroupViewModels));

            return knownContactGroupViewModels.Any(knownContactGroupViewModel => knownContactGroupViewModel.Number == contactGroupViewModel.Number);
        }

        public static SelectListItem SelectListItemFor(this ContactGroupViewModel contactGroupViewModel, bool selected)
        {
            NullGuard.NotNull(contactGroupViewModel, nameof(contactGroupViewModel));

            return new SelectListItem(contactGroupViewModel.Name, Convert.ToString(contactGroupViewModel.Number), selected);
        }

        public static IEnumerable<SelectListItem> SelectListFor(this IEnumerable<ContactGroupViewModel> contactGroupViewModels, int? selectedValue)
        {
            NullGuard.NotNull(contactGroupViewModels, nameof(contactGroupViewModels));

            return contactGroupViewModels.Select(contactGroupViewModel => contactGroupViewModel.SelectListItemFor(selectedValue.HasValue && selectedValue.Value == contactGroupViewModel.Number)).ToArray();
        }
    }
}