using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

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