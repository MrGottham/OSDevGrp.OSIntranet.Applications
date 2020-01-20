using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public abstract class ContactQueryHandlerBase<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IContactQuery
    {
        #region Constructor

        protected ContactQueryHandlerBase(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository));

            Validator = validator;
            MicrosoftGraphRepository = microsoftGraphRepository;
            ContactRepository = contactRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IMicrosoftGraphRepository MicrosoftGraphRepository { get; }

        protected IContactRepository ContactRepository { get; }

        #endregion

        #region Methods

        public async Task<TResult> QueryAsync(TQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(Validator);

            return await GetDataAsync(query, query.ToToken());
        }

        protected abstract Task<TResult> GetDataAsync(TQuery query, IRefreshableToken token);

        #endregion
    }
}