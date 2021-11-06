using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Logic
{
    public class HashKeyGenerator : IHashKeyGenerator
    {
        #region Methods

        public Task<string> ComputeHashAsync(IEnumerable<byte> byteCollection)
        {
            NullGuard.NotNull(byteCollection, nameof(byteCollection));

            return Task.Run(() =>
            {
                using SHA512 sha512Hash = SHA512.Create();
                byte[] hash = sha512Hash.ComputeHash(byteCollection.ToArray());

                return Convert.ToBase64String(hash);
            });
        }

        #endregion
    }
}