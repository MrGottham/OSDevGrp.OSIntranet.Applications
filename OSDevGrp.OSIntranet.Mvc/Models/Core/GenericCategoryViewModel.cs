using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Core
{
	public class GenericCategoryViewModel : AuditableViewModelBase
    {
        #region Constructor

        public GenericCategoryViewModel()
        {
        }

        private GenericCategoryViewModel(string header, string controller, string submitText, string submitAction, string cancelText, string cancelAction, EditMode editMode)
        {
	        NullGuard.NotNullOrWhiteSpace(header, nameof(header))
		        .NotNullOrWhiteSpace(controller, nameof(controller))
		        .NotNullOrWhiteSpace(submitText, nameof(submitText))
		        .NotNullOrWhiteSpace(submitAction, nameof(submitAction))
		        .NotNullOrWhiteSpace(cancelText, nameof(cancelText))
		        .NotNullOrWhiteSpace(cancelAction, nameof(cancelAction));

	        Header = header;
	        Controller = controller;
	        SubmitText = submitText;
	        SubmitAction = submitAction;
	        CancelText = cancelText;
	        CancelAction = cancelAction;
	        EditMode = editMode;
        }

        #endregion

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

        public string Header { get; }

        public string Controller { get; }

        public string SubmitText { get; }

        public string SubmitAction { get; }

        public string CancelText { get; }

        public string CancelAction { get; }

		#endregion

		#region Methods

		internal static GenericCategoryViewModel Create(string header, string controller, string createAction, string cancelAction)
		{
			NullGuard.NotNullOrWhiteSpace(header, nameof(header))
				.NotNullOrWhiteSpace(controller, nameof(controller))
				.NotNullOrWhiteSpace(createAction, nameof(createAction))
				.NotNullOrWhiteSpace(cancelAction, nameof(cancelAction));

			return new GenericCategoryViewModel(header, controller, "Opret", createAction, "Fortryd", cancelAction, EditMode.Create);
		}

		internal static GenericCategoryViewModel Create<TGenericCategory>(TGenericCategory genericCategory, IConverter converter) where TGenericCategory : IGenericCategory
		{
			NullGuard.NotNull(genericCategory, nameof(genericCategory))
				.NotNull(converter, nameof(converter));

			return converter.Convert<TGenericCategory, GenericCategoryViewModel>(genericCategory);
		}

		internal static GenericCategoryViewModel Create<TGenericCategory>(string header, string controller, string updateAction, string cancelAction, TGenericCategory genericCategory) where TGenericCategory : IGenericCategory
		{
			NullGuard.NotNullOrWhiteSpace(header, nameof(header))
				.NotNullOrWhiteSpace(controller, nameof(controller))
				.NotNullOrWhiteSpace(updateAction, nameof(updateAction))
				.NotNullOrWhiteSpace(cancelAction, nameof(cancelAction))
				.NotNull(genericCategory, nameof(genericCategory));

			return new GenericCategoryViewModel(header, controller, "Opdatér", updateAction, "Fortryd", cancelAction, EditMode.Edit)
			{
				Number = genericCategory.Number,
				Name = genericCategory.Name,
                Deletable = genericCategory.Deletable,
				CreatedByIdentifier = genericCategory.CreatedByIdentifier,
				CreatedDateTime = genericCategory.CreatedDateTime,
				ModifiedByIdentifier = genericCategory.ModifiedByIdentifier,
				ModifiedDateTime = genericCategory.ModifiedDateTime
			};
		}

        #endregion
	}

	internal static class GenericCategoryViewModelExtensions
    {
        #region Methods

        internal static IActionResult AsView(this GenericCategoryViewModel genericCategoryViewModel, Controller controller)
        {
	        NullGuard.NotNull(genericCategoryViewModel, nameof(genericCategoryViewModel))
		        .NotNull(controller, nameof(controller));

	        switch (genericCategoryViewModel.EditMode)
	        {
				case EditMode.Create:
					return controller.View("CreateGenericCategory", genericCategoryViewModel);

                case EditMode.Edit:
                    return controller.View("UpdateGenericCategory", genericCategoryViewModel);

				default:
					throw new NotSupportedException($"Unhandled value: {genericCategoryViewModel.EditMode}");
	        }
		}

        internal static string GetDeletionUrl(this GenericCategoryViewModel genericCategoryViewModel, string controller, string deleteAction, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(genericCategoryViewModel, nameof(genericCategoryViewModel))
                .NotNullOrWhiteSpace(controller, nameof(controller))
                .NotNullOrWhiteSpace(deleteAction, nameof(deleteAction))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction(deleteAction, controller);
        }

        internal static string GetDeletionData(this GenericCategoryViewModel genericCategoryViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(genericCategoryViewModel, nameof(genericCategoryViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"number: {genericCategoryViewModel.Number}, {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        #endregion
    }
}