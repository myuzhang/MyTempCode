using System.Collections.Generic;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace GarminIntegration
{
    public static class GarminFactories
    {
        public static GarminConnector GetGarminConnector(Dictionary<string, string> accessTokenSecrets)
        {
            var credential = GarminSettingsHelpers.Instance.GetCredential();
            var tokenManager = new InMemoryTokenManager(credential.Key, credential.Secret);
            tokenManager.StoreLocalSecrets(accessTokenSecrets);
            return new GarminConnector(tokenManager);
        }

        public static ServiceProviderDescription GetHmacSha1ServiceDescription() =>
            new ServiceProviderDescription
            {
                RequestTokenEndpoint =
                    new MessageReceivingEndpoint(
                        GarminSettingsHelpers.Instance.GetProviderEndpoint("requestTokenEndpoint"),
                        HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.PostRequest),
                UserAuthorizationEndpoint =
                    new MessageReceivingEndpoint(
                        GarminSettingsHelpers.Instance.GetProviderEndpoint("userAuthorizationEndpoint"),
                        HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
                AccessTokenEndpoint =
                    new MessageReceivingEndpoint(
                        GarminSettingsHelpers.Instance.GetProviderEndpoint("accessTokenEndpoint"),
                        HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.PostRequest),
                TamperProtectionElements =
                    new ITamperProtectionChannelBindingElement[]
                        {new HmacSha1SigningBindingElement()},
            };

        public static MessageReceivingEndpoint DailySummaries =>
            new MessageReceivingEndpoint(
                GarminSettingsHelpers.Instance.GetMessageReceivingEndpoint(nameof(DailySummaries)),
                HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest);

        public static MessageReceivingEndpoint ActivitySummaries => new MessageReceivingEndpoint(
            GarminSettingsHelpers.Instance.GetMessageReceivingEndpoint(nameof(ActivitySummaries)),
                HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest);

        public static MessageReceivingEndpoint BackfillSummaries => new MessageReceivingEndpoint(
            GarminSettingsHelpers.Instance.GetMessageReceivingEndpoint(nameof(BackfillSummaries)),
            HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest);

        public static MessageReceivingEndpoint SleepSummaries => new MessageReceivingEndpoint(
            GarminSettingsHelpers.Instance.GetMessageReceivingEndpoint(nameof(SleepSummaries)),
            HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest);

        public static MessageReceivingEndpoint EpochSummaries => new MessageReceivingEndpoint(
            GarminSettingsHelpers.Instance.GetMessageReceivingEndpoint(nameof(EpochSummaries)),
            HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest);

        public static MessageReceivingEndpoint BodyCompositionSummaries => new MessageReceivingEndpoint(
            GarminSettingsHelpers.Instance.GetMessageReceivingEndpoint(nameof(BodyCompositionSummaries)),
            HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest);

    }
}
