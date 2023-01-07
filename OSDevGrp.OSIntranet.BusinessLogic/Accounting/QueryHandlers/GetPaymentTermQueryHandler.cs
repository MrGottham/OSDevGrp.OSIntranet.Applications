using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class GetPaymentTermQueryHandler : IQueryHandler<IGetPaymentTermQuery, IPaymentTerm>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IClaimResolver _claimResolver;
        private readonly IAccountingRepository _accountingRepository;

        #endregion

        #region Constructor

        public GetPaymentTermQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            _validator = validator;
            _claimResolver = claimResolver;
            _accountingRepository = accountingRepository;
        }

        #endregion

        #region Methods

        public async Task<IPaymentTerm> QueryAsync(IGetPaymentTermQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _accountingRepository);

            IPaymentTerm paymentTerm = await _accountingRepository.GetPaymentTermAsync(query.Number);
            if (paymentTerm == null)
            {
                return null;
            }

            if (_claimResolver.IsAccountingAdministrator())
            {
                return paymentTerm;
            }

            paymentTerm.ApplyProtection();

            return paymentTerm;
        }

        #endregion
    }
}
