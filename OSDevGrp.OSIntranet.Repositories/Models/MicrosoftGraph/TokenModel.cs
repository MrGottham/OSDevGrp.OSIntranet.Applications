using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph
{
	[DataContract]
    internal class TokenModel
    {
        #region Properties

        [DataMember(Name = "token_type", IsRequired = true, EmitDefaultValue = true)]
        public string TokenType { get; set; }

        [DataMember(Name = "scope", IsRequired = true, EmitDefaultValue = true)]
        public string Scope { get; set; }

        [DataMember(Name = "expires_in", IsRequired = true, EmitDefaultValue = true)]
        public int ExpiresIn { get; set; }

        [DataMember(Name = "access_token", IsRequired = true, EmitDefaultValue = true)]
        public string AccessToken { get; set; }

        [DataMember(Name = "refresh_token", IsRequired = true, EmitDefaultValue = true)]
        public string RefreshToken { get; set; }

        [IgnoreDataMember] 
        public DateTime Expires { get; protected set; }

        #endregion

        #region Methods

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            NullGuard.NotNull(context, nameof(context));

            Expires = DateTime.UtcNow.AddSeconds(ExpiresIn);
        }

        #endregion
    }

    internal static class TokenModelExtensions
    {
        internal static IRefreshableToken ToDomain(this TokenModel tokenModel)
        {
            NullGuard.NotNull(tokenModel, nameof(tokenModel));

            return RefreshableTokenFactory.Create()
	            .WithTokenType(tokenModel.TokenType)
	            .WithAccessToken(tokenModel.AccessToken)
	            .WithRefreshToken(tokenModel.RefreshToken)
	            .WithExpires(tokenModel.Expires)
	            .Build();
        }
    }
}