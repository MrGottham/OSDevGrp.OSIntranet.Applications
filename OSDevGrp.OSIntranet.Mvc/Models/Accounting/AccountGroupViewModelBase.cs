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
    public abstract class AccountGroupViewModelBase : AuditableViewModelBase
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

        #region Methods

        public abstract string GetDeletionLink(IUrlHelper urlHelper);

        public string GetDeletionData(IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"number: {Number}, {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        #endregion
    }

    public static class AccountGroupViewModelBaseExtensions
    {
        public static bool IsKnownAccountGroup<TAccountGroupViewModel>(this TAccountGroupViewModel accountGroupViewModel, IEnumerable<TAccountGroupViewModel> knownAccountGroupViewModels) where TAccountGroupViewModel : AccountGroupViewModelBase
        {
            NullGuard.NotNull(accountGroupViewModel, nameof(accountGroupViewModel))
                .NotNull(knownAccountGroupViewModels, nameof(knownAccountGroupViewModels));

            return knownAccountGroupViewModels.Any(knownAccountGroupViewModel => knownAccountGroupViewModel.Number == accountGroupViewModel.Number);
        }

        public static SelectListItem SelectListItemFor<TAccountGroupViewModel>(this TAccountGroupViewModel accountGroupViewModel, bool selected) where TAccountGroupViewModel : AccountGroupViewModelBase
        {
            NullGuard.NotNull(accountGroupViewModel, nameof(accountGroupViewModel));

            return new SelectListItem(accountGroupViewModel.Name, Convert.ToString(accountGroupViewModel.Number), selected);
        }

        public static IEnumerable<SelectListItem> SelectListFor<TAccountGroupViewModel>(this IEnumerable<TAccountGroupViewModel> accountGroupViewModels, int? selectedValue) where TAccountGroupViewModel : AccountGroupViewModelBase
        {
            NullGuard.NotNull(accountGroupViewModels, nameof(accountGroupViewModels));

            return accountGroupViewModels.Select(accountGroupViewModel => accountGroupViewModel.SelectListItemFor(selectedValue.HasValue && selectedValue.Value == accountGroupViewModel.Number)).ToArray();
        }
    }
}