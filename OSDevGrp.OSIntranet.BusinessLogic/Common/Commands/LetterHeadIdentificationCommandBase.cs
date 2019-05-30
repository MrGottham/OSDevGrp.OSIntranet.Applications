using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public abstract class LetterHeadIdentificationCommandBase : ILetterHeadIdentificationCommand
    {
        #region Private variables

        private ILetterHead _letterHead;

        #endregion

        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return validator.Integer.ShouldBeBetween(Number, 1, 99, GetType(), nameof(Number));
        }

        protected Task<ILetterHead> GetLetterHead(ICommonRepository commonRepository)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            return Task.Run(async () =>  _letterHead ?? (_letterHead = await commonRepository.GetLetterHeadAsync(Number)));
        }

        #endregion
    }
}