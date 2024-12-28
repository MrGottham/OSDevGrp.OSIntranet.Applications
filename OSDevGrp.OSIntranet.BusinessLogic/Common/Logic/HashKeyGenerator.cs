using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Logic
{
    public class HashKeyGenerator : IHashKeyGenerator
    {
        #region Methods

        public Task<string> ComputeHashAsync(IEnumerable<byte> byteCollection)
        {
            NullGuard.NotNull(byteCollection, nameof(byteCollection));

            return Task.Run(() => Convert.ToBase64String(byteCollection.ToArray().ComputeSha512Hash()));
        }

        #endregion
    }
}