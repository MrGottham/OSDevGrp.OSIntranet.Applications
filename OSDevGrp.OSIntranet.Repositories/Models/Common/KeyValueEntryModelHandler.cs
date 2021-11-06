using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
    internal class KeyValueEntryModelHandler : ModelHandlerBase<IKeyValueEntry, RepositoryContext, KeyValueEntryModel, string>
    {
        #region Constructor

        public KeyValueEntryModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<KeyValueEntryModel> Entities => DbContext.KeyValueEntries;

        protected override Func<IKeyValueEntry, string> PrimaryKey => keyValueEntry => keyValueEntry.Key;

        #endregion

        #region Methods

        internal Task<IKeyValueEntry> PullAsync(string key)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            return ReadAsync(key);
        }

        internal async Task<IKeyValueEntry> PushAsync(IKeyValueEntry keyValueEntry)
        {
            NullGuard.NotNull(keyValueEntry, nameof(keyValueEntry));

            if (await ReadAsync(keyValueEntry.Key) == null)
            {
                return await CreateAsync(keyValueEntry);
            }

            return await UpdateAsync(keyValueEntry);
        }

        protected override Expression<Func<KeyValueEntryModel, bool>> EntitySelector(string key) => keyValueEntryModel => keyValueEntryModel.Key == key;

        protected override Task<IEnumerable<IKeyValueEntry>> SortAsync(IEnumerable<IKeyValueEntry> keyValueEntryCollection)
        {
            NullGuard.NotNull(keyValueEntryCollection, nameof(keyValueEntryCollection));

            return Task.FromResult(keyValueEntryCollection.OrderBy(m => m.Key).AsEnumerable());
        }

        protected override async Task<KeyValueEntryModel> OnReadAsync(KeyValueEntryModel keyValueEntryModel)
        {
            NullGuard.NotNull(keyValueEntryModel, nameof(keyValueEntryModel));

            keyValueEntryModel.Deletable = await CanDeleteAsync(keyValueEntryModel);

            return keyValueEntryModel;
        }

        protected override Task OnUpdateAsync(IKeyValueEntry keyValueEntry, KeyValueEntryModel keyValueEntryModel)
        {
            NullGuard.NotNull(keyValueEntry, nameof(keyValueEntry))
                .NotNull(keyValueEntryModel, nameof(keyValueEntryModel));

            keyValueEntryModel.Value = keyValueEntry.ToBase64();

            return Task.CompletedTask;
        }

        protected override Task<bool> CanDeleteAsync(KeyValueEntryModel keyValueEntryModel)
        {
            NullGuard.NotNull(keyValueEntryModel, nameof(keyValueEntryModel));

            return Task.FromResult(true);
        }

        #endregion
    }
}