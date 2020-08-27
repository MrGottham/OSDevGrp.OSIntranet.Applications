using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Common
{
    public class LetterHeadViewModel : AuditableViewModelBase
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

        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved")]
        [Required(ErrorMessage = "Brevhovedets 1. linje skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "Længden på brevhovedets 1. linje skal være mellem {2} og {1} tegn.")]
        public string Line1 { get; set; }

        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved")]
        [StringLength(64, MinimumLength = 0, ErrorMessage = "Længden på brevhovedets 2. linje skal være mellem {2} og {1} tegn.")]
        public string Line2 { get; set; }

        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved")]
        [StringLength(64, MinimumLength = 0, ErrorMessage = "Længden på brevhovedets 3. linje skal være mellem {2} og {1} tegn.")]
        public string Line3 { get; set; }

        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved")]
        [StringLength(64, MinimumLength = 0, ErrorMessage = "Længden på brevhovedets 4. linje skal være mellem {2} og {1} tegn.")]
        public string Line4 { get; set; }

        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved")]
        [StringLength(64, MinimumLength = 0, ErrorMessage = "Længden på brevhovedets 5. linje skal være mellem {2} og {1} tegn.")]
        public string Line5 { get; set; }

        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved")]
        [StringLength(64, MinimumLength = 0, ErrorMessage = "Længden på brevhovedets 6. linje skal være mellem {2} og {1} tegn.")]
        public string Line6 { get; set; }

        [Display(Name = "Brevhoved", ShortName = "Brevhoved", Description = "Brevhoved")]
        [StringLength(64, MinimumLength = 0, ErrorMessage = "Længden på brevhovedets 7. linje skal være mellem {2} og {1} tegn.")]
        public string Line7 { get; set; }

        [Display(Name = "Tekst for CVR-nummer", ShortName = "CVR-nr.", Description = "Tekst for CVR-nummer")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "Længden på teksten til CVR-nummeret skal være mellem {2} og {1} tegn.")]
        public string CompanyIdentificationNumber { get ; set; }

        public bool Deletable { get; set; }

        #endregion
    }

    public static class LetterHeadViewModelExtensions
    {
        public static string GetDeletionLink(this LetterHeadViewModel letterHeadViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(letterHeadViewModel, nameof(letterHeadViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteLetterHead", "Common");
        }

        public static string GetDeletionData(this LetterHeadViewModel letterHeadViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(letterHeadViewModel, nameof(letterHeadViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"number: {letterHeadViewModel.Number}, {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        public static bool IsKnownLetterHead(this LetterHeadViewModel letterHeadViewModel, IEnumerable<LetterHeadViewModel> knownLetterHeadViewModels)
        {
            NullGuard.NotNull(letterHeadViewModel, nameof(letterHeadViewModel))
                .NotNull(knownLetterHeadViewModels, nameof(knownLetterHeadViewModels));

            return knownLetterHeadViewModels.Any(knownLetterHeadViewModel => knownLetterHeadViewModel.Number == letterHeadViewModel.Number);
        }

        public static SelectListItem SelectListItemFor(this LetterHeadViewModel letterHeadViewModel, bool selected)
        {
            NullGuard.NotNull(letterHeadViewModel, nameof(letterHeadViewModel));

            return new SelectListItem(letterHeadViewModel.Name, Convert.ToString(letterHeadViewModel.Number), selected);
        }

        public static IEnumerable<SelectListItem> SelectListFor(this IEnumerable<LetterHeadViewModel> letterHeadViewModels, int? selectedValue)
        {
            NullGuard.NotNull(letterHeadViewModels, nameof(letterHeadViewModels));

            return letterHeadViewModels.Select(letterHeadViewModel => letterHeadViewModel.SelectListItemFor(selectedValue.HasValue && selectedValue.Value == letterHeadViewModel.Number)).ToArray();
        }
    }
}