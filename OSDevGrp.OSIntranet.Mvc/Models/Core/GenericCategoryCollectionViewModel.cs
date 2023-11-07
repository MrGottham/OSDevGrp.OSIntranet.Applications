using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OSDevGrp.OSIntranet.Mvc.Models.Core
{
    public class GenericCategoryCollectionViewModel : ReadOnlyCollection<GenericCategoryViewModel>
    {
        #region Constructor

        private GenericCategoryCollectionViewModel(string header, string controller, string createAction, string updateAction, Func<GenericCategoryViewModel, IUrlHelper, string> deletionUrlGetter, bool allowCreation, bool allowModification, bool allowDeletion, IList<GenericCategoryViewModel> genericCategoryViewModelCollection)
            : base(genericCategoryViewModelCollection)
        {
            NullGuard.NotNullOrWhiteSpace(header, nameof(header))
                .NotNullOrWhiteSpace(controller, nameof(controller))
                .NotNullOrWhiteSpace(createAction, nameof(createAction))
                .NotNullOrWhiteSpace(updateAction, nameof(updateAction))
                .NotNull(deletionUrlGetter, nameof(deletionUrlGetter));

            Header = header;
            Controller = controller;
            CreateAction = createAction;
            UpdateAction = updateAction;
            DeletionUrlGetter = deletionUrlGetter;
            AllowCreation = allowCreation;
            AllowModification = allowModification;
            AllowDeletion = allowDeletion;
        }

        #endregion

        #region Properties

        public string Header { get; }

        public string Controller { get; }

        public string CreateAction { get; }

        public string UpdateAction { get; }

        public Func<GenericCategoryViewModel, IUrlHelper, string> DeletionUrlGetter { get; }

        public bool AllowCreation { get; }

        public bool AllowModification { get; }

        public bool AllowDeletion { get; }

        #endregion

		#region Methods

		internal static GenericCategoryCollectionViewModel Create<TGenericCategory>(string header, string controller, string createAction, string updateAction, Func<GenericCategoryViewModel, IUrlHelper, string> deletionUrlGetter, IEnumerable<TGenericCategory> genericCategoryCollection, IConverter converter, bool allowCreation = true, bool allowModification = true, bool allowDeletion = true) where TGenericCategory : IGenericCategory
        {
            NullGuard.NotNullOrWhiteSpace(header, nameof(header))
                .NotNullOrWhiteSpace(controller, nameof(controller))
                .NotNullOrWhiteSpace(createAction, nameof(createAction))
                .NotNullOrWhiteSpace(updateAction, nameof(updateAction))
                .NotNull(deletionUrlGetter, nameof(deletionUrlGetter))
                .NotNull(genericCategoryCollection, nameof(genericCategoryCollection))
                .NotNull(converter, nameof(converter));

            return new GenericCategoryCollectionViewModel(header, controller, createAction, updateAction, deletionUrlGetter, allowCreation, allowModification, allowDeletion, genericCategoryCollection.Select(genericCategory => GenericCategoryViewModel.Create(genericCategory, converter)).ToList());
        }

        #endregion
    }

    internal static class GenericCategoryCollectionViewModelExtensions
    {
        #region Methods

        internal static ViewResult AsView(this GenericCategoryCollectionViewModel genericCategoryCollectionViewModel, Controller controller)
        {
            NullGuard.NotNull(genericCategoryCollectionViewModel, nameof(genericCategoryCollectionViewModel))
                .NotNull(controller, nameof(controller));

            return controller.View("GenericCategories", genericCategoryCollectionViewModel);
        }

        #endregion
    }
}