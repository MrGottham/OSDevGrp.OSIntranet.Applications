using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Tests
{
    public abstract class RepositoryTestBase : IDisposable
    {
        #region Private and protected variables

        private readonly IList<IDisposable> _disposables = new List<IDisposable>();
        private static IEventPublisher _eventPublisher;
        protected static readonly object SyncRoot = new();

        #endregion

        #region Methods

        public void Dispose()
        {
            lock (SyncRoot)
            {
                foreach (IDisposable disposable in _disposables)
                {
                    disposable.Dispose();
                }
            }
        }

        protected void RegisterDisposable(IDisposable disposable)
        {
            NullGuard.NotNull(disposable, nameof(disposable));

            lock (SyncRoot)
            {
                _disposables.Add(disposable);
            }
        }

        protected IConfiguration CreateTestConfiguration()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<RepositoryTestBase>()
                .Build();
        }

        protected IConverterFactory CreateConverterFactory()
        {
            return new ConverterFactory(CreateLicensesOptions(), CreateLoggerFactory());
        }

        protected IOptions<LicensesOptions> CreateLicensesOptions()
        {
            return Microsoft.Extensions.Options.Options.Create(CreateTestConfiguration().GetLicensesOptions());
        }

        protected ILoggerFactory CreateLoggerFactory()
        {
            return NullLoggerFactory.Instance;
        }

        protected IEventPublisher CreateEventPublisher()
        {
            lock (SyncRoot)
            {
                return _eventPublisher ??= new EventPublisher();
            }
        }

        #endregion
    }
}