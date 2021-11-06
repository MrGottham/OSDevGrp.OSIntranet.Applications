using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Logic
{
    public class KeyGenerator : IKeyGenerator
    {
        #region Private variables

        private readonly IHashKeyGenerator _hashKeyGenerator;
        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        public KeyGenerator(IHashKeyGenerator hashKeyGenerator, IClaimResolver claimResolver)
        {
            NullGuard.NotNull(hashKeyGenerator, nameof(hashKeyGenerator))
                .NotNull(claimResolver, nameof(claimResolver));

            _hashKeyGenerator = hashKeyGenerator;
            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        public Task<string> GenerateGenericKeyAsync(IEnumerable<string> keyElementCollection)
        {
            NullGuard.NotNull(keyElementCollection, nameof(keyElementCollection));

            string[] keyElementArray = keyElementCollection.ToArray();

            ValidateKeyElementCollection(keyElementArray, typeof(IEnumerable<string>), nameof(keyElementCollection));

            return _hashKeyGenerator.ComputeHashAsync(ToByteArray(Join(keyElementArray)));
        }

        public Task<string> GenerateUserSpecificKeyAsync(IEnumerable<string> keyElementCollection)
        {
            NullGuard.NotNull(keyElementCollection, nameof(keyElementCollection));

            string[] keyElementArray = keyElementCollection.ToArray();

            ValidateKeyElementCollection(keyElementArray, typeof(IEnumerable<string>), nameof(keyElementCollection));

            string userIdentifier = GetUserIdentifier();

            return _hashKeyGenerator.ComputeHashAsync(ToByteArray(Join(userIdentifier, keyElementArray)));
        }

        private string GetUserIdentifier()
        {
            string userIdentifier = _claimResolver.GetMailAddress();
            if (string.IsNullOrWhiteSpace(userIdentifier) == false)
            {
                return userIdentifier;
            }

            userIdentifier = _claimResolver.GetNameIdentifier();
            if (string.IsNullOrWhiteSpace(userIdentifier) == false)
            {
                return userIdentifier;
            }

            throw new IntranetExceptionBuilder(ErrorCode.UnableToResolveUserIdentifier).Build();
        }

        private static void ValidateKeyElementCollection(IEnumerable<string> keyElementCollection, Type argumentType, string argumentName)
        {
            NullGuard.NotNull(keyElementCollection, nameof(keyElementCollection))
                .NotNull(argumentType, nameof(argumentType))
                .NotNullOrWhiteSpace(argumentName, nameof(argumentName));

            if (keyElementCollection.Any())
            {
                return;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldContainSomeItems, argumentName)
                .WithValidatingType(argumentType)
                .WithValidatingField(argumentName)
                .Build();
        }

        private static string Join(string userIdentifier, params string[] keyElementCollection)
        {
            NullGuard.NotNullOrWhiteSpace(userIdentifier, nameof(userIdentifier))
                .NotNull(keyElementCollection, nameof(keyElementCollection));

            return $"{userIdentifier}|{Join(keyElementCollection)}";
        }

        private static string Join(params string[] keyElementCollection)
        {
            NullGuard.NotNull(keyElementCollection, nameof(keyElementCollection));

            return string.Join('|', keyElementCollection);
        }

        private static byte[] ToByteArray(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return Encoding.UTF8.GetBytes(value);
        }

        #endregion
    }
}