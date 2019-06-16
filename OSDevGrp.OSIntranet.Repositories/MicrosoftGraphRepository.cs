using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class MicrosoftGraphRepository : TokenBasedWebRepositoryBase<IRefreshableToken>, IMicrosoftGraphRepository
    {
        #region Private constants

        private const string MicrosoftLoginUrl = "https://login.microsoftonline.com";
        private const string MicrosoftGraphUrl = "https://graph.microsoft.com/v1.0";
        private const string Scope = "User.Read Contacts.ReadWrite offline_access";

        #endregion

        #region Private variables
        
        private IRefreshableToken _currentToken;
        private readonly ConverterBase _microsoftGraphModelConverter = new MicrosoftGraphModelConverter();

        #endregion

        #region Constructor

        public MicrosoftGraphRepository(IConfiguration configuration, IPrincipalResolver principalResolver, ILogger<MicrosoftGraphRepository> logger)
            : base(configuration, principalResolver, logger)
        {
        }

        #endregion

        #region Properties

        protected override string AuthorizeUrl
        {
            get
            {
                string tenant = Configuration["Security:Microsoft:Tenant"];
                return $"{MicrosoftLoginUrl}/{tenant}/oauth2/v2.0/authorize";
            }
        }

        protected override string TokenUrl
        {
            get
            {
                string tenant = Configuration["Security:Microsoft:Tenant"];
                return $"{MicrosoftLoginUrl}/{tenant}/oauth2/v2.0/token";
            }
        }

        protected override IRefreshableToken Token
        {
            get => _currentToken;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _currentToken = value;
            }
        }

        #endregion

        #region Methods

        public Task<Uri> GetAuthorizeUriAsync(Uri redirectUri, Guid stateIdentifier)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri));

            return GetAuthorizeUriAsync(redirectUri, Scope, stateIdentifier.ToString("D"));
        }

        public async Task<IRefreshableToken> AcquireTokenAsync(Uri redirectUri, string code)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(code, nameof(code));

            TokenModel tokenModel = await AcquireTokenAsync<TokenModel>(redirectUri, Scope, code);

            Token = _microsoftGraphModelConverter.Convert<TokenModel, IRefreshableToken>(tokenModel);

            return Token;
        }

        public async Task<IRefreshableToken> RefreshTokenAsync(Uri redirectUri, IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNull(refreshableToken, nameof(refreshableToken));

            TokenModel tokenModel = await RefreshTokenAsync<TokenModel>(redirectUri, Scope, refreshableToken);

            Token = _microsoftGraphModelConverter.Convert<TokenModel, IRefreshableToken>(tokenModel);

            return Token;
        }

        public async Task<IEnumerable<IContact>> GetContacts(IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken));

            Token = refreshableToken;

            List<IContact> contacts = new List<IContact>();

            ContactCollectionModel contactCollectionModel = await GetAsync<ContactCollectionModel>(new Uri($"{MicrosoftGraphUrl}/me/contacts"), null, CreateSerializerSettings());
            contacts.AddRange(_microsoftGraphModelConverter.Convert<ContactCollectionModel, IEnumerable<IContact>>(contactCollectionModel));

            while (string.IsNullOrWhiteSpace(contactCollectionModel.NextLink) == false)
            {
                contactCollectionModel = await GetAsync<ContactCollectionModel>(new Uri(contactCollectionModel.NextLink), null, CreateSerializerSettings());
                contacts.AddRange(_microsoftGraphModelConverter.Convert<ContactCollectionModel, IEnumerable<IContact>>(contactCollectionModel));
            }

            return contacts.OrderBy(m => m.Name.DisplayName).ToList();
        }

        public async Task<IContact> GetContact(IRefreshableToken refreshableToken, string identifier)
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken))
                .NotNullOrWhiteSpace(identifier, nameof(identifier));

            Token = refreshableToken;

            ContactModel contactModel = await GetAsync<ContactModel>(new Uri($"{MicrosoftGraphUrl}/me/contacts/{identifier}"), null, CreateSerializerSettings());
            if (contactModel == null)
            {
                return null;
            }

            return _microsoftGraphModelConverter.Convert<ContactModel, IContact>(contactModel);
        }

        protected override IDictionary<string, string> GetParametersToAuthorize(Uri redirectUri, string scope, string state)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNullOrWhiteSpace(state, nameof(state));

            IDictionary<string, string> parametersToAuthorize = base.GetParametersToAuthorize(redirectUri, scope, state);
            parametersToAuthorize.Add("client_id", Configuration["Security:Microsoft:ClientId"]);
            parametersToAuthorize.Add("response_type", "code");
            parametersToAuthorize.Add("redirect_uri", redirectUri.AbsoluteUri);
            parametersToAuthorize.Add("scope", scope);
            parametersToAuthorize.Add("response_mode", "query");
            parametersToAuthorize.Add("state", state);
            return parametersToAuthorize;
        }

        protected override void SetupRequestToAcquireToken(HttpRequestMessage httpRequestMessage, Uri redirectUri, string scope, string code)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNullOrWhiteSpace(code, nameof(code));

            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", Configuration["Security:Microsoft:ClientId"]},
                {"grant_type", "authorization_code"},
                {"scope", scope},
                {"code", code},
                {"redirect_uri", redirectUri.AbsoluteUri},
                {"client_secret", Configuration["Security:Microsoft:ClientSecret"]},
            };

            httpRequestMessage.Content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

        protected override void SetupRequestToRefreshToken(HttpRequestMessage httpRequestMessage, Uri redirectUri, string scope, IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNull(refreshableToken, nameof(refreshableToken));

            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", Configuration["Security:Microsoft:ClientId"]},
                {"grant_type", "refresh_token"},
                {"scope", scope},
                {"refresh_token", refreshableToken.RefreshToken},
                {"redirect_uri", redirectUri.AbsoluteUri},
                {"client_secret", Configuration["Security:Microsoft:ClientSecret"]},
            };

            httpRequestMessage.Content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

        protected override void SetupAuthenticationForRequest(HttpRequestMessage httpRequestMessage, IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(httpRequestMessage, nameof(httpRequestMessage))
                .NotNull(refreshableToken, nameof(refreshableToken));

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(refreshableToken.TokenType, refreshableToken.AccessToken);
        }

        private DataContractJsonSerializerSettings CreateSerializerSettings()
        {
            return new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ssZ")
            };
        }

        #endregion
    }
}
