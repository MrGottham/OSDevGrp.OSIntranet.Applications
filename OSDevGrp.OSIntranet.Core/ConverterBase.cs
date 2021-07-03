using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core
{
    public abstract class ConverterBase : IConverter
    {
        #region Private variables

        private static readonly IDictionary<Type, MapperConfiguration> MapperConfigurations = new Dictionary<Type, MapperConfiguration>();
        private static readonly object SyncRoot = new object();

        #endregion

        #region Protected variables

        protected readonly IMapper Mapper;

        #endregion

        #region Constructor

        protected ConverterBase()
        {
            MapperConfiguration mapperConfiguration = ResolveMapperConfiguration(GetType());

            Mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Properties

        protected virtual IDictionary<string, object> StateDictionary => new ConcurrentDictionary<string, object>();

        #endregion

        #region Methods

        public TTarget Convert<TSource, TTarget>(TSource source)
        {
            NullGuard.NotNull(source, nameof(source));

            try
            {
                return Mapper.Map<TSource, TTarget>(source, opt =>
                {
                    foreach (KeyValuePair<string, object> state in StateDictionary)
                    {
                        opt.Items[state.Key] = state.Value;
                    }
                });
            }
            catch (AutoMapperMappingException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        protected abstract void Initialize(IMapperConfigurationExpression mapperConfiguration);

        private MapperConfiguration ResolveMapperConfiguration(Type converterType)
        {
            NullGuard.NotNull(converterType, nameof(converterType));

            lock (SyncRoot)
            {
                if (MapperConfigurations.TryGetValue(converterType, out MapperConfiguration mapperConfiguration))
                {
                    return mapperConfiguration;
                }

                mapperConfiguration = new MapperConfiguration(Initialize);
                MapperConfigurations.Add(converterType, mapperConfiguration);
                return mapperConfiguration;
            }
        }

        #endregion
    }
}