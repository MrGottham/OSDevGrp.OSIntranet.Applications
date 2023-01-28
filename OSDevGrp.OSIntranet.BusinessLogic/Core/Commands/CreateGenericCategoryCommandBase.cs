using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.Commands
{
    internal abstract class CreateGenericCategoryCommandBase<TGenericCategory> : GenericCategoryDataCommandBase<TGenericCategory>, ICreateGenericCategoryCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected CreateGenericCategoryCommandBase(int number, string name) 
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
                .Object.ShouldBeUnknownValue(Number, number => IsGenericCategoryUnknownAsync(number, genericCategoryGetter), GetType(), nameof(Number));
        }

        #endregion
    }
}