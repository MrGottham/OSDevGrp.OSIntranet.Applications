using OSDevGrp.OSIntranet.BusinessLogic.Core.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.Commands
{
    internal abstract class GenericCategoryIdentificationCommandBase<TGenericCategory> : IGenericCategoryIdentificationCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected GenericCategoryIdentificationCommandBase(int number)
        {
            Number = number;
        }

        #endregion

        #region Properties

        public int Number { get; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, Func<int, Task<TGenericCategory>> genericCategoryGetter)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(genericCategoryGetter, nameof(genericCategoryGetter));

            return validator.ValidateGenericCategoryIdentifier(Number, GetType(), nameof(Number));
        }

        protected static async Task<bool> IsGenericCategoryKnownAsync(int number, Func<int, Task<TGenericCategory>> genericCategoryGetter)
        {
            NullGuard.NotNull(genericCategoryGetter, nameof(genericCategoryGetter));

            return await GetGenericCategoryAsync(number, genericCategoryGetter) != null;
        }

        protected static async Task<bool> IsGenericCategoryUnknownAsync(int number, Func<int, Task<TGenericCategory>> genericCategoryGetter)
        {
            NullGuard.NotNull(genericCategoryGetter, nameof(genericCategoryGetter));

            return await GetGenericCategoryAsync(number, genericCategoryGetter) == null;
        }

        protected static Task<TGenericCategory> GetGenericCategoryAsync(int number, Func<int, Task<TGenericCategory>> genericCategoryGetter)
        {
            NullGuard.NotNull(genericCategoryGetter, nameof(genericCategoryGetter));

            return genericCategoryGetter(number);
        }

        #endregion
    }
}