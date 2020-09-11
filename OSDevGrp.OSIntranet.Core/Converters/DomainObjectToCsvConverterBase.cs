using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Converters
{
    public abstract class DomainObjectToCsvConverterBase<TDomainObject> : ColumnValueAppenderBase, IDomainObjectToCsvConverter<TDomainObject> where TDomainObject : class
    {
        #region Constructor

        protected DomainObjectToCsvConverterBase(IFormatProvider formatProvider)
            : base(formatProvider)
        {
        }

        #endregion

        #region Methods

        public abstract Task<IEnumerable<string>> GetColumnNamesAsync();

        public abstract Task<IEnumerable<string>> ConvertAsync(TDomainObject domainObject);

        #endregion
    }
}