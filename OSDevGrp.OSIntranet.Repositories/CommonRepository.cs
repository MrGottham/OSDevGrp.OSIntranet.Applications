using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal class CommonRepository : DatabaseRepositoryBase<RepositoryContext>, ICommonRepository
    {
        #region Constructor

        public CommonRepository(RepositoryContext repositoryContext, IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(repositoryContext, configuration, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<ILetterHead>> GetLetterHeadsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(DbContext, CommonModelConverter.Create());
                    return await letterHeadModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> GetLetterHeadAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(DbContext, CommonModelConverter.Create());
                    return await letterHeadModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> CreateLetterHeadAsync(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return ExecuteAsync(async () =>
                {
                    LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(DbContext, CommonModelConverter.Create());
                    return await letterHeadModelHandler.CreateAsync(letterHead);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> UpdateLetterHeadAsync(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(DbContext, CommonModelConverter.Create());
                    return await letterHeadModelHandler.UpdateAsync(letterHead);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILetterHead> DeleteLetterHeadAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using LetterHeadModelHandler letterHeadModelHandler = new LetterHeadModelHandler(DbContext, CommonModelConverter.Create());
                    return await letterHeadModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IKeyValueEntry> PullKeyValueEntryAsync(string key)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            return ExecuteAsync(async () =>
                {
                    using KeyValueEntryModelHandler keyValueEntryModelHandler = new KeyValueEntryModelHandler(DbContext, CommonModelConverter.Create());
                    return await keyValueEntryModelHandler.PullAsync(key);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IKeyValueEntry> PushKeyValueEntryAsync(IKeyValueEntry keyValueEntry)
        {
            NullGuard.NotNull(keyValueEntry, nameof(keyValueEntry));

            return ExecuteAsync(async () =>
                {
                    using KeyValueEntryModelHandler keyValueEntryModelHandler = new KeyValueEntryModelHandler(DbContext, CommonModelConverter.Create());
                    return await keyValueEntryModelHandler.PushAsync(keyValueEntry);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IKeyValueEntry> DeleteKeyValueEntryAsync(string key)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            return ExecuteAsync(async () =>
                {
                    using KeyValueEntryModelHandler keyValueEntryModelHandler = new KeyValueEntryModelHandler(DbContext, CommonModelConverter.Create());
                    return await keyValueEntryModelHandler.DeleteAsync(key);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<INationality>> GetNationalitiesAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using NationalityModelHandler handler = new NationalityModelHandler(DbContext, CommonModelConverter.Create());
                    return await handler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<INationality> GetNationalityAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using NationalityModelHandler handler = new NationalityModelHandler(DbContext, CommonModelConverter.Create());
                    return await handler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task CreateNationalityAsync(INationality nationality)
        {
            NullGuard.NotNull(nationality, nameof(nationality));

            return ExecuteAsync(async () =>
                {
                    using NationalityModelHandler handler = new NationalityModelHandler(DbContext, CommonModelConverter.Create());
                    await handler.CreateAsync(nationality);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task UpdateNationalityAsync(INationality nationality)
        {
            NullGuard.NotNull(nationality, nameof(nationality));

            return ExecuteAsync(async () =>
                {
                    using NationalityModelHandler handler = new NationalityModelHandler(DbContext, CommonModelConverter.Create());
                    await handler.UpdateAsync(nationality);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task DeleteNationalityAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using NationalityModelHandler handler = new NationalityModelHandler(DbContext, CommonModelConverter.Create());
                    await handler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<ILanguage>> GetLanguagesAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using LanguageModelHandler handler = new LanguageModelHandler(DbContext, CommonModelConverter.Create());
                    return await handler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ILanguage> GetLanguageAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using LanguageModelHandler handler = new LanguageModelHandler(DbContext, CommonModelConverter.Create());
                    return await handler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task CreateLanguageAsync(ILanguage language)
        {
            NullGuard.NotNull(language, nameof(language));

            return ExecuteAsync(async () =>
                {
                    using LanguageModelHandler handler = new LanguageModelHandler(DbContext, CommonModelConverter.Create());
                    await handler.CreateAsync(language);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task UpdateLanguageAsync(ILanguage language)
        {
            NullGuard.NotNull(language, nameof(language));

            return ExecuteAsync(async () =>
                {
                    using LanguageModelHandler handler = new LanguageModelHandler(DbContext, CommonModelConverter.Create());
                    await handler.UpdateAsync(language);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task DeleteLanguageAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using LanguageModelHandler handler = new LanguageModelHandler(DbContext, CommonModelConverter.Create());
                    await handler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}