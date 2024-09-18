using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class AuthorizationCodeGenerator : IAuthorizationCodeGenerator
    {
        #region Private variables

        private readonly IKeyGenerator _keyGenerator;
        private readonly IAuthorizationCodeFactory _authorizationCodeFactory;

        #endregion

        #region Constructor

        public AuthorizationCodeGenerator(IKeyGenerator keyGenerator, IAuthorizationCodeFactory authorizationCodeFactory)
        {
            NullGuard.NotNull(keyGenerator, nameof(keyGenerator))
                .NotNull(authorizationCodeFactory, nameof(authorizationCodeFactory));

            _keyGenerator = keyGenerator;
            _authorizationCodeFactory = authorizationCodeFactory;
        }

        #endregion

        #region Methods

        public async Task<IAuthorizationCode> GenerateAsync()
        {
            DateTimeOffset expires = DateTimeOffset.UtcNow.AddMinutes(10);
            string genericKey = await _keyGenerator.GenerateGenericKeyAsync(new[] {Guid.NewGuid().ToString("D"), expires.ToString("O", CultureInfo.InvariantCulture)});

            return _authorizationCodeFactory.Create(genericKey, expires).Build();
        }

        #endregion
    }
}