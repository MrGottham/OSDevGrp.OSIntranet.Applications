using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Common;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class CommonRepository : RepositoryBase, ICommonRepository
    {
        #region Constructor

        public CommonRepository(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<ILetterHead>> GetLetterHeadsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(CreateRepositoryContext(), CommonModelConverter.Create());
                    return await letterHeadModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> GetLetterHeadAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(CreateRepositoryContext(), CommonModelConverter.Create());
                    return await letterHeadModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> CreateLetterHeadAsync(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return ExecuteAsync(async () =>
                {
                    LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(CreateRepositoryContext(), CommonModelConverter.Create());
                    return await letterHeadModelHandler.CreateAsync(letterHead);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> UpdateLetterHeadAsync(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(CreateRepositoryContext(), CommonModelConverter.Create());
                    return await letterHeadModelHandler.UpdateAsync(letterHead);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> DeleteLetterHeadAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(CreateRepositoryContext(), CommonModelConverter.Create());
                    return await letterHeadModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}