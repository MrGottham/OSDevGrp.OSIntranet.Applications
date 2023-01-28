using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.Commands
{
    internal abstract class UpdateGenericCategoryCommandBase<TGenericCategory> : GenericCategoryDataCommandBase<TGenericCategory>, IUpdateGenericCategoryCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected UpdateGenericCategoryCommandBase(int number, string name) 
            : base(number, name)
        {
        }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, Func<int, Task<TGenericCategory>> genericCategoryGetter)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(genericCategoryGetter, nameof(genericCategoryGetter));

            return base.Validate(validator, genericCategoryGetter)
                .Object.ShouldBeKnownValue(Number, number => IsGenericCategoryKnownAsync(number, genericCategoryGetter), GetType(), nameof(Number));
        }

        #endregion
    }
}