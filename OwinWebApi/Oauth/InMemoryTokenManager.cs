using System;
using System.Collections.Generic;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;

namespace Oauth
{
    public class InMemoryTokenManager : IConsumerTokenManager
    {
        private readonly Dictionary<string, string> _tokensAndSecrets;

        public InMemoryTokenManager(string consumerKey, string consumeSecret)
        {
            _tokensAndSecrets = new Dictionary<string, string>();
            ConsumerSecret = consumeSecret;
            ConsumerKey = consumerKey;
        }

        public string ConsumerKey { get; }

        public string ConsumerSecret { get; }

        public string GetTokenSecret(string token)
        {
            if (_tokensAndSecrets.ContainsKey(token))
            {
                return _tokensAndSecrets[token];
            }
            return null;
        }

        public void StoreLocalSecrets(string accessToken, string accessSecret) =>
            _tokensAndSecrets[accessToken] = accessSecret;

        public void StoreLocalSecrets(Dictionary<string, string> tokensAndSecrets) =>
            _tokensAndSecrets.AddRange(tokensAndSecrets);

        public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response) =>
            _tokensAndSecrets[response.Token] = response.TokenSecret;

        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken,
            string accessToken,
            string accessTokenSecret)
        {
            _tokensAndSecrets.Remove(requestToken);
            _tokensAndSecrets[accessToken] = accessTokenSecret;
        }

        public TokenType GetTokenType(string token)
        {
            throw new NotImplementedException();
        }
    }
}
