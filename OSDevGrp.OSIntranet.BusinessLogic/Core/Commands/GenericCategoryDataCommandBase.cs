using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.Commands
{
    internal abstract class GenericCategoryDataCommandBase<TGenericCategory> : GenericCategoryIdentificationCommandBase<TGenericCategory>, IGenericCategoryDataCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected GenericCategoryDataCommandBase(int number, string name) 
            : base(number)
        {
            Name = name;
        }

        #endregion

        #region Properties

        public string Name { get; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, Func<int, Task<TGenericCategory>> genericCategoryGetter)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(genericCategoryGetter, nameof(genericCategoryGetter));

            return base.Validate(validator, genericCategoryGetter)
                .String.ShouldNotBeNullOrWhiteSpace(Name, GetType(), nameof(Name))
                .String.ShouldHaveMinLength(Name, 1, GetType(), nameof(Name))
                .String.ShouldHaveMaxLength(Name, 256, GetType(), nameof(Name));
        }

        public abstract TGenericCategory ToDomain();

        #endregion
    }
}