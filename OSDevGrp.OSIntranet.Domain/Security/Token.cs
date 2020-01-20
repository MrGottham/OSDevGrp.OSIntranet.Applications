using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    [Serializable]
    public class Token : IToken
    {
        #region Constructors

        public Token(string tokenType, string accessToken, DateTime expires)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken));

            TokenType = tokenType;
            AccessToken = accessToken;
            Expires = expires;
        }

        #endregion

        #region Properties

        public string TokenType { get; }

        public string AccessToken { get; }

        public DateTime Expires { get; }

        #endregion

        #region Methods

        public string ToBase64()
        {
            using MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, this);

            memoryStream.Seek(0, SeekOrigin.Begin);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static TToken Create<TToken>(string base64String) where TToken : class, IToken
        {
            NullGuard.NotNullOrWhiteSpace(base64String, nameof(base64String));

            using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(base64String));
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (TToken) binaryFormatter.Deserialize(memoryStream);
        }

        public static IToken Create(ITokenBasedQuery tokenBasedQuery)
        {
            NullGuard.NotNull(tokenBasedQuery, nameof(tokenBasedQuery));

            return new Token(tokenBasedQuery.TokenType, tokenBasedQuery.AccessToken, tokenBasedQuery.Expires);
        }

        public static IToken Create(ITokenBasedCommand tokenBasedCommand)
        {
            NullGuard.NotNull(tokenBasedCommand, nameof(tokenBasedCommand));

            return new Token(tokenBasedCommand.TokenType, tokenBasedCommand.AccessToken, tokenBasedCommand.Expires);
        }

        #endregion
    }
}
