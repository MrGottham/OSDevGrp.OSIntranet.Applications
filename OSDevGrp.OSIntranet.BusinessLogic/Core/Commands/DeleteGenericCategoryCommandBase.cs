﻿using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.Commands
{
    internal abstract class DeleteGenericCategoryCommandBase<TGenericCategory> : GenericCategoryIdentificationCommandBase<TGenericCategory>, IDeleteGenericCategoryCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected DeleteGenericCategoryCommandBase(int number) 
            : base(number)
        {
        }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, Func<int, Task<TGenericCategory>> genericCategoryGetter)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(genericCategoryGetter, nameof(genericCategoryGetter));

            return base.Validate(validator, genericCategoryGetter)
                .Object.ShouldBeKnownValue(Number, number => IsGenericCategoryKnownAsync(number, genericCategoryGetter), GetType(), nameof(Number))
                .Object.ShouldBeDeletable(Number, number => GetGenericCategoryAsync(number, genericCategoryGetter), GetType(), nameof(Number));
        }

        #endregion
    }
}