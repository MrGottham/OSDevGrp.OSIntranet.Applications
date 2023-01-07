using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class GetPaymentTermCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<IPaymentTerm>>
    {
        #region Private variables

        private readonly IAccountingRepository _accountingRepository;
        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        public GetPaymentTermCollectionQueryHandler(IAccountingRepository accountingRepository, IClaimResolver claimResolver)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(claimResolver, nameof(claimResolver));

            _accountingRepository = accountingRepository;
            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<IPaymentTerm>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IPaymentTerm[] paymentTermCollection = (await _accountingRepository.GetPaymentTermsAsync() ?? Array.Empty<IPaymentTerm>()).ToArray();
            if (paymentTermCollection.Length == 0)
            {
                return paymentTermCollection;
            }

            if (_claimResolver.IsAccountingAdministrator())
            {
                return paymentTermCollection;
            }

            foreach (IPaymentTerm paymentTerm in paymentTermCollection)
            {
                paymentTerm.ApplyProtection();
            }

            return paymentTermCollection;
        }

        #endregion
    }
}