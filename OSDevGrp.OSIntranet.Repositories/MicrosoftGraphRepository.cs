﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph;
using OSDevGrp.OSIntranet.Repositories.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal class MicrosoftGraphRepository : TokenBasedWebRepositoryBase<IRefreshableToken>, IMicrosoftGraphRepository
    {
        #region Private constants

        private const string MicrosoftLoginUrl = "https://login.microsoftonline.com";
        private const string MicrosoftGraphUrl = "https://graph.microsoft.com/v1.0";
        private const string Scope = "User.Read Contacts.ReadWrite offline_access";

        #endregion

        #region Private variables

        private IRefreshableToken _currentToken;
        private readonly IOptions<MicrosoftSecurityOptions> _microsoftSecurityOptions;

        #endregion

        #region Constructor

        public MicrosoftGraphRepository(IOptions<MicrosoftSecurityOptions> microsoftSecurityOptions, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NullGuard.NotNull(microsoftSecurityOptions, nameof(microsoftSecurityOptions));

            _microsoftSecurityOptions = microsoftSecurityOptions;
        }

        #endregion

        #region Properties

        protected override string AuthorizeUrl => $"{MicrosoftLoginUrl}/{_microsoftSecurityOptions.Value.Tenant}/oauth2/v2.0/authorize";

        protected override string TokenUrl => $"{MicrosoftLoginUrl}/{_microsoftSecurityOptions.Value.Tenant}/oauth2/v2.0/token";

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

            IConverter microsoftGraphModelConverter = MicrosoftGraphModelConverter.Create();

            TokenModel tokenModel = await AcquireTokenAsync<TokenModel>(redirectUri, Scope, code);

            Token = microsoftGraphModelConverter.Convert<TokenModel, IRefreshableToken>(tokenModel);

            return Token;
        }

        public async Task<IRefreshableToken> RefreshTokenAsync(Uri redirectUri, IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNull(refreshableToken, nameof(refreshableToken));

            IConverter microsoftGraphModelConverter = MicrosoftGraphModelConverter.Create();

            TokenModel tokenModel = await RefreshTokenAsync<TokenModel>(redirectUri, Scope, refreshableToken);

            Token = microsoftGraphModelConverter.Convert<TokenModel, IRefreshableToken>(tokenModel);

            return Token;
        }

        public async Task<IEnumerable<IContact>> GetContactsAsync(IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken));

            IConverter microsoftGraphModelConverter = MicrosoftGraphModelConverter.Create();

            Token = refreshableToken;

            List<IContact> contacts = new List<IContact>();

            ContactCollectionModel contactCollectionModel = await GetAsync<ContactCollectionModel>(new Uri($"{MicrosoftGraphUrl}/me/contacts"), null, CreateSerializerSettings());
            contacts.AddRange(microsoftGraphModelConverter.Convert<ContactCollectionModel, IEnumerable<IContact>>(contactCollectionModel));

            while (string.IsNullOrWhiteSpace(contactCollectionModel.NextLink) == false)
            {
                contactCollectionModel = await GetAsync<ContactCollectionModel>(new Uri(contactCollectionModel.NextLink), null, CreateSerializerSettings());
                contacts.AddRange(microsoftGraphModelConverter.Convert<ContactCollectionModel, IEnumerable<IContact>>(contactCollectionModel));
            }

            return contacts.OrderBy(m => m.Name.DisplayName).ToList();
        }

        public async Task<IContact> GetContactAsync(IRefreshableToken refreshableToken, string identifier)
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken))
                .NotNullOrWhiteSpace(identifier, nameof(identifier));

            IConverter microsoftGraphModelConverter = MicrosoftGraphModelConverter.Create();

            Token = refreshableToken;

            ContactModel contactModel = await GetContactModelAsync(identifier, CreateSerializerSettings());
            if (contactModel == null)
            {
                return null;
            }

            return microsoftGraphModelConverter.Convert<ContactModel, IContact>(contactModel);
        }

        public async Task<IContact> CreateContactAsync(IRefreshableToken refreshableToken, IContact contact)
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken))
                .NotNull(contact, nameof(contact));

            IConverter microsoftGraphModelConverter = MicrosoftGraphModelConverter.Create();

            Token = refreshableToken;

            ContactModel contactModel = microsoftGraphModelConverter.Convert<IContact, ContactModel>(contact);

            DataContractJsonSerializerSettings serializerSettings = CreateSerializerSettings();

            ContactModel createdContactModel = await PostAsync<ContactModel>(new Uri($"{MicrosoftGraphUrl}/me/contacts"), httpRequestMessage => httpRequestMessage.Content = ModelToStringContent(contactModel, CreateSerializerSettings("yyyy-MM-dd")), serializerSettings);

            return microsoftGraphModelConverter.Convert<ContactModel, IContact>(createdContactModel);
        }

        public async Task<IContact> UpdateContactAsync(IRefreshableToken refreshableToken, IContact contact)
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken))
                .NotNull(contact, nameof(contact));

            IConverter microsoftGraphModelConverter = MicrosoftGraphModelConverter.Create();

            Token = refreshableToken;

            DataContractJsonSerializerSettings serializerSettings = CreateSerializerSettings();

            ContactModel sourceContactModel = await GetContactModelAsync(contact.ExternalIdentifier, serializerSettings);
            if (sourceContactModel == null)
            {
                return null;
            }

            ContactModel targetContactModel = microsoftGraphModelConverter.Convert<IContact, ContactModel>(contact);

            ContactModel updatedContactModel = await PatchAsync<ContactModel>(new Uri($"{MicrosoftGraphUrl}/me/contacts/{targetContactModel.Identifier}"), httpRequestMessage => httpRequestMessage.Content = ModelToStringContent(targetContactModel.ToChangedOnlyModel(sourceContactModel), CreateSerializerSettings("yyyy-MM-dd")), serializerSettings);

            return microsoftGraphModelConverter.Convert<ContactModel, IContact>(updatedContactModel);
        }

        public async Task DeleteContactAsync(IRefreshableToken refreshableToken, string identifier)
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken))
                .NotNullOrWhiteSpace(identifier, nameof(identifier));

            Token = refreshableToken;

            ContactModel contactModel = await GetContactModelAsync(identifier, CreateSerializerSettings());
            if (contactModel == null)
            {
                return;
            }

            await DeleteAsync(new Uri($"{MicrosoftGraphUrl}/me/contacts/{contactModel.Identifier}"));
        }

        protected override IDictionary<string, string> GetParametersToAuthorize(Uri redirectUri, string scope, string state)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNullOrWhiteSpace(state, nameof(state));

            IDictionary<string, string> parametersToAuthorize = base.GetParametersToAuthorize(redirectUri, scope, state);
            parametersToAuthorize.Add("client_id", _microsoftSecurityOptions.Value.ClientId);
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
                {"client_id", _microsoftSecurityOptions.Value.ClientId},
                {"grant_type", "authorization_code"},
                {"scope", scope},
                {"code", code},
                {"redirect_uri", redirectUri.AbsoluteUri},
                {"client_secret", _microsoftSecurityOptions.Value.ClientSecret},
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
                {"client_id", _microsoftSecurityOptions.Value.ClientId},
                {"grant_type", "refresh_token"},
                {"scope", scope},
                {"refresh_token", refreshableToken.RefreshToken},
                {"redirect_uri", redirectUri.AbsoluteUri},
                {"client_secret", _microsoftSecurityOptions.Value.ClientSecret},
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

        private async Task<ContactModel> GetContactModelAsync(string identifier, DataContractJsonSerializerSettings serializerSettings)
        {
            NullGuard.NotNullOrWhiteSpace(identifier, nameof(identifier))
                .NotNull(serializerSettings, nameof(serializerSettings));

            return await GetAsync<ContactModel>(new Uri($"{MicrosoftGraphUrl}/me/contacts/{identifier}"), null, serializerSettings);
        }

        private DataContractJsonSerializerSettings CreateSerializerSettings(string dateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ") 
        {
            return new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat(dateTimeFormat)
            };
        }

        private StringContent ModelToStringContent<T>(T model, DataContractJsonSerializerSettings serializerSettings) where T : class
        {
            NullGuard.NotNull(model, nameof(model))
                .NotNull(serializerSettings, nameof(serializerSettings));

            using MemoryStream memoryStream = new MemoryStream();

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(model.GetType(), serializerSettings);
            serializer.WriteObject(memoryStream, model);

            memoryStream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new StreamReader(memoryStream);
            return new StringContent(reader.ReadToEnd(), Encoding.UTF8, "application/json");
        }

        #endregion
    }
}