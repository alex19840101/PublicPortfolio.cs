using Grpc.Core;

namespace NotificationsSender
{
    internal static class RequestHeadersPreparator
    {
        internal static Metadata? GetMetadataWithAuthorizationHeader(string? jwt)
        {
            if (jwt == null)
                return null;

            var requestHeaders = new Metadata
            {
                { "Authorization", $"Bearer {jwt}" }
            };

            return requestHeaders;
        }
    }
}
