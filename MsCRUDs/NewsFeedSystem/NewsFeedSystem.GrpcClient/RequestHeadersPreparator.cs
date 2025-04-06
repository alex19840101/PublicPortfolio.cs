using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace NewsFeedSystem.GrpcClient
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
